using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace MealPlannerEngine
{
	public partial class MealPlannerEngine : ServiceBase
	{
		public MealPlannerEngine()
		{
			InitializeComponent();

			eventLog = new EventLog();
			if ( !EventLog.SourceExists( "Meal Planner Engine Source" ) )
			{
				EventLog.CreateEventSource( "Meal Planner Engine Source", "MealPlannerLog" );
			}
			eventLog.Source = "Meal Planner Engine Source";
			eventLog.Log = "MealPlannerLog";
		}

		protected override void OnStart( string[] args )
		{
			eventLog.WriteEntry( "Starting service engine", EventLogEntryType.Information, 0 );

			_engineService = new MealPlannerEngineService();
			_engineServiceHost = ConfigureEngineService( _engineService );

			// Set up a timer to trigger every hour.
			_updateTimer = new Timer();
			_updateTimer.Interval = 60000;//* 60; // 60 seconds * 60 minutes
			_updateTimer.Elapsed += new ElapsedEventHandler( this.OnTimer );
			_updateTimer.Start();

			HandleMealPlan();

			eventLog.WriteEntry( "Starting service engine -- Complete", EventLogEntryType.Information, 2 );
		}

		public void OnTimer( object sender, System.Timers.ElapsedEventArgs args )
		{
			HandleMealPlan();
		}

		protected override void OnStop()
		{
			eventLog.WriteEntry( "Stopping service engine", EventLogEntryType.Information, 3 );

			_engineServiceHost.Close();

			_updateTimer.Dispose();

			eventLog.WriteEntry( "Stopping service engine -- Complete", EventLogEntryType.Information, 4 );
		}

		private void HandleMealPlan()
		{
			eventLog.WriteEntry( "Managing meal plan", EventLogEntryType.Information, 5 );

			var mealPlan = new Serializer().GetMealPlan();
			var mealOptions = new Serializer().GetMealOptions();
			var config = new Serializer().GetConfiguration();

			var earliestAllowedMoment = DateTime.Now.Date;

			if ( mealPlan.MealPlanDays.Any( d => d.Day < earliestAllowedMoment ) )
			{
				mealPlan.MealPlanDays = mealPlan.MealPlanDays.Where( d => d.Day >= earliestAllowedMoment ).ToList();

				new Serializer().SetMealPlan( mealPlan );
			}

			if ( !IsPlanSufficient( mealPlan, config ) )
			{
				new NotifyIconServiceChannel( eventLog ).SendMealPlanDaysNeeded( config.DaysToPlan );
				return; //Don't spam multiple messages
			}

			if ( !IsShoppingSufficient( mealPlan, config ) )
			{
				new NotifyIconServiceChannel( eventLog ).SendShoppingNeeded( config.ShoppingDaysNeeded );
				return; //Don't spam multiple messages
			}
		}

		private bool IsPlanSufficient( MealPlan mealPlan, MealPlannerConfiguration config )
		{
			for ( var i = 0; i < config.DaysToPlan; ++i )
			{
				var dateNeeded = DateTime.Now.Date.AddDays( i );

				if ( !mealPlan.MealPlanDays.Any( d => d.Day.Date == dateNeeded ) )
					return false;
			}

			return true;
		}

		private bool IsShoppingSufficient( MealPlan mealPlan, MealPlannerConfiguration config )
		{
			for ( var i = 0; i < config.ShoppingDaysNeeded; ++i )
			{
				var dateToCheck = DateTime.Now.Date.AddDays( i );

				var datePlan = mealPlan.MealPlanDays.FirstOrDefault( d => d.Day.Date == dateToCheck );

				if ( datePlan != null && !datePlan.ShoppedFor )
					return false;
			}

			return true;
		}

		private WebServiceHost ConfigureEngineService( IMealPlannerEngineService engineService )
		{
			var config = new Serializer().GetConfiguration();
			var serviceUri = String.Format( "http://{0}:{1}", config.EngineService.HostName, config.EngineService.Port );

			eventLog.WriteEntry( String.Format( "Hosting Meal Planner Engine Service at {0}", serviceUri ),
				EventLogEntryType.Information, 1 );

			Uri serviceAddress = new Uri( serviceUri );
			var engineServiceHost = new WebServiceHost( engineService, serviceAddress );

			engineServiceHost.Open();

			return engineServiceHost;
		}

		private IMealPlannerEngineService _engineService;
		private WebServiceHost _engineServiceHost;
		private Timer _updateTimer;
	}
}
