using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotifyIconMealPlanner
{
	class MealPlannerNotifyIcon
	{
		public MealPlannerNotifyIcon()
		{
			_notifyIcon = new NotifyIcon();

			_notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
			_notifyIcon.BalloonTipText = "Your meal plan needs an update. Click here to do it now.";
			_notifyIcon.BalloonTipText = "Meal plan update";

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

		private void OnIconDoubleClick(object sender, EventArgs e)
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
