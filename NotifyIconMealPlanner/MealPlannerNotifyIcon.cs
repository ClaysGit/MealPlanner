using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotifyIconMealPlanner
{
	interface IMealPlannerNotifyIcon
	{
		void ShowPopupForPlanDaysNeeded( int daysNeeded );
		void ShowPopupForShoppingNeeded( int daysLeft );
	}

	class MealPlannerNotifyIcon : IMealPlannerNotifyIcon
	{
		public MealPlannerNotifyIcon()
		{
			_notifyIcon = new NotifyIcon();

			_notifyIcon.Text = "Meal planner management";
			_notifyIcon.Icon = new System.Drawing.Icon( @".\apple.ico" );

			var contextMenu = new ContextMenuStrip();
			contextMenu.SuspendLayout();

			var closeMenuItem = new ToolStripMenuItem();

			contextMenu.Items.Add( closeMenuItem );
			contextMenu.Name = "Meal Planner Context menu";
			contextMenu.Size = new Size( 153, 70 );

			closeMenuItem.Name = "Meal Planner Close Menu Item";
			closeMenuItem.Size = new Size( 152, 22 );
			closeMenuItem.Text = "Quit";
			closeMenuItem.Click += OnCloseClick;

			contextMenu.ResumeLayout( false );
			_notifyIcon.ContextMenuStrip = contextMenu;

			_notifyIcon.MouseDoubleClick += OnIconDoubleClick;
			Application.ApplicationExit += OnApplicationExit;

			_notifyIcon.Visible = true;
		}

		public void ShowPopupForPlanDaysNeeded(int daysNeeded)
		{
			_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
			_notifyIcon.BalloonTipText = "Your meal plan needs an update. Click here to do it now.";
			_notifyIcon.BalloonTipTitle = "Meal plan update";
			_notifyIcon.BalloonTipClicked += OnBalloonTipClick;

			//1000 ms * 60 = 60s
			//60s * 10 = 10m
			_notifyIcon.ShowBalloonTip( 1000 * 60 * 10 );
		}

		public void ShowPopupForShoppingNeeded(int daysLeft)
		{
			_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
			_notifyIcon.BalloonTipText = "Your meal plan needs an update. Click here to do it now.";
			_notifyIcon.BalloonTipTitle = "Meal plan update";
			_notifyIcon.BalloonTipClicked += OnBalloonTipClick;

			//1000 ms * 60 = 60s
			//60s * 10 = 10m
			_notifyIcon.ShowBalloonTip( 1000 * 60 * 10 );
		}

		private void OnIconDoubleClick(object sender, EventArgs e)
		{
			OpenWebApp();
		}

		private void OnBalloonTipClick( object sender, EventArgs e )
		{
			OpenWebApp();
		}

		private void OpenWebApp()
		{
			System.Diagnostics.Process.Start( "http://localhost:17575/" );
		}

		private void OnCloseClick(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void OnApplicationExit( object sender, EventArgs e )
		{
			_notifyIcon.Visible = false;
		}

		private NotifyIcon _notifyIcon;
	}
}
