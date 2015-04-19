using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

using MealPlanner.Library;

namespace NotifyIconMealPlanner
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			_mealPlannerNotifyIcon = new MealPlannerNotifyIcon();
			_notifyIconService = new NotifyIconService( _mealPlannerNotifyIcon );
			_notifyIconServiceHost = ConfigureNotifyIconService( _notifyIconService );

			Application.ApplicationExit += OnApplicationExit;
			Application.Run();
		}

		private static WebServiceHost ConfigureNotifyIconService( INotifyIconService notifyIconService )
		{
			var config = new Serializer().GetConfiguration();

			Uri serviceAddress = new Uri( "http://localhost:17576" );
			var notifyIconServiceHost = new WebServiceHost( notifyIconService, serviceAddress );

			notifyIconServiceHost.Open();

			return notifyIconServiceHost;
		}

		private static void OnApplicationExit( object sender, EventArgs e )
		{
			_notifyIconServiceHost.Close();
		}

		private static IMealPlannerNotifyIcon _mealPlannerNotifyIcon;
		private static INotifyIconService _notifyIconService;
		private static WebServiceHost _notifyIconServiceHost;
	}
}
