using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using MealPlanner.Library;

namespace NotifyIconMealPlanner
{
	interface IMealPlannerNotifyIcon
	{
		void ShowPopupForPlanDaysNeeded( int daysNeeded );
		void ShowPopupForShoppingNeeded( int daysLeft );
	}

	class MealPlannerNotifyIcon : IMealPlannerNotifyIcon
	{
		public MealPlannerNotifyIcon( string uri )
		{
			_suppressBefore = DateTime.MinValue;

			_eventLog = new EventLog();
			if ( !EventLog.SourceExists( "Notify Icon Source" ) )
			{
				EventLog.CreateEventSource( "Notify Icon Source", "MealPlannerLog" );
			}
			_eventLog.Source = "Notify Icon Source";
			_eventLog.Log = "MealPlannerLog";

			_eventLog.WriteEntry( "Initializing Icon", EventLogEntryType.Information, 0 );
			_eventLog.WriteEntry( "Service hosted at " + uri, EventLogEntryType.Information, 0 );

			_notifyIcon = new NotifyIcon();

			_notifyIcon.Text = "Meal planner management";
			_notifyIcon.Icon = new System.Drawing.Icon( @".\apple.ico" );

			var contextMenu = new ContextMenuStrip();
			contextMenu.SuspendLayout();

			var closeMenuItem = new ToolStripMenuItem();
			var delayMenuItem = new ToolStripMenuItem();

			contextMenu.Items.Add( closeMenuItem );
			contextMenu.Name = "Meal Planner Context menu";
			contextMenu.Size = new Size( 153, 70 );

			closeMenuItem.Name = "Meal Planner Close Menu Item";
			closeMenuItem.Size = new Size( 152, 22 );
			closeMenuItem.Text = "Quit";
			closeMenuItem.Click += OnCloseClick;

			delayMenuItem.Name = "Meal Planner Delay Menu Item";
			delayMenuItem.Size = new Size( 152, 22 );
			delayMenuItem.Text = "Not today...";
			delayMenuItem.Click += OnDelayClick;

			contextMenu.ResumeLayout( false );
			_notifyIcon.ContextMenuStrip = contextMenu;

			_notifyIcon.MouseDoubleClick += OnIconDoubleClick;
			Application.ApplicationExit += OnApplicationExit;

			_notifyIcon.Visible = true;
		}

		public void ShowPopupForPlanDaysNeeded( int daysNeeded )
		{
			_eventLog.WriteEntry( "Received request for planning pop up", EventLogEntryType.Information, 1 );

			if ( DateTime.Now < _suppressBefore )
				return;

			_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
			_notifyIcon.BalloonTipText = String.Format( "Your meal plan needs {0} days planned out. Click here to do it now.", daysNeeded );
			_notifyIcon.BalloonTipTitle = "Meal plan update";
			_notifyIcon.BalloonTipClicked += OnPlanningBalloonTipClick;

			//1000 ms * 60 = 60s
			//60s * 10 = 10m
			_notifyIcon.ShowBalloonTip( 1000 * 60 * 10 );
		}

		public void ShowPopupForShoppingNeeded( int daysLeft )
		{
			_eventLog.WriteEntry( "Received request for shopping pop up", EventLogEntryType.Information, 2 );

			if ( DateTime.Now < _suppressBefore )
				return;

			_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
			_notifyIcon.BalloonTipText = String.Format( "You need to go shopping! You have {0} days left. Click here to set it up.", daysLeft );
			_notifyIcon.BalloonTipTitle = "Meal plan update";
			_notifyIcon.BalloonTipClicked += OnShoppingBalloonTipClick;

			//1000 ms * 60 = 60s
			//60s * 10 = 10m
			_notifyIcon.ShowBalloonTip( 1000 * 60 * 10 );
		}

		private void OnIconDoubleClick( object sender, EventArgs e )
		{
			OpenWebApp();
		}

		private void OnPlanningBalloonTipClick( object sender, EventArgs e )
		{
			OpenWebApp();
		}

		private void OnShoppingBalloonTipClick( object sender, EventArgs e )
		{
			OpenWebApp( "ShoppingList" );
		}

		private void OpenWebApp( string route = "" )
		{
			var config = new Serializer().GetConfiguration();
			var serviceConfig = config.WebService;
			var serviceUri = String.Format( "http://{0}:{1}/{2}", serviceConfig.HostName, serviceConfig.Port, route );
			System.Diagnostics.Process.Start( serviceUri );
		}

		private void OnCloseClick( object sender, EventArgs e )
		{
			Application.Exit();
		}

		private void OnDelayClick( object sender, EventArgs e )
		{
			_eventLog.WriteEntry( "Delaying pop ups for one day", EventLogEntryType.Information, 2 );

			_suppressBefore = DateTime.Now.Date.AddDays( 1 );
		}

		private void OnApplicationExit( object sender, EventArgs e )
		{
			_notifyIcon.Visible = false;
		}

		private NotifyIcon _notifyIcon;
		private DateTime _suppressBefore;
		private EventLog _eventLog;
	}
}
