using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

			eventLog1 = new EventLog();
			if ( System.Diagnostics.EventLog.SourceExists( "MealPlannerEngineSource" ) )
			{
				System.Diagnostics.EventLog.CreateEventSource(
					"MealPlannerEngineSource", "MealPlannerEngineLog" );
			}
			eventLog1.Source = "MealPlannerEngineSource";
			eventLog1.Log = "MealPlannerEngineLog";
		}

		protected override void OnStart( string[] args )
		{
			eventLog1.WriteEntry( "Starting service engine", EventLogEntryType.Information, 0 );

			// Set up a timer to trigger every hour.
			_updateTimer = new Timer();
			_updateTimer.Interval = 60000 * 60; // 60 seconds * 60 minutes
			_updateTimer.Elapsed += new ElapsedEventHandler( this.OnTimer );
			_updateTimer.Start();

			HandleMealPlan();

			eventLog1.WriteEntry( "Starting service engine -- Complete", EventLogEntryType.Information, 1 );
		}

		public void OnTimer( object sender, System.Timers.ElapsedEventArgs args )
		{
			HandleMealPlan();
		}

		protected override void OnStop()
		{
			eventLog1.WriteEntry( "Stopping service engine", EventLogEntryType.Information, 2 );

			_updateTimer.Dispose();

			eventLog1.WriteEntry( "Stopping service engine -- Complete", EventLogEntryType.Information, 3 );
		}

		private void HandleMealPlan()
		{
			eventLog1.WriteEntry( "Managing meal plan", EventLogEntryType.Information, 3 );

			var mealPlan = new Serializer().GetMealPlan();
			var mealOptions = new Serializer().GetMealOptions();

			var earliestAllowedMoment = DateTime.Now.Date;

			if ( mealPlan.MealPlanDays.Any( d => d.Day < earliestAllowedMoment ) )
			{
				mealPlan.MealPlanDays = mealPlan.MealPlanDays.Where( d => d.Day >= earliestAllowedMoment ).ToList();

				new Serializer().SetMealPlan( mealPlan );
			}

			if ( !IsPlanSufficient( mealPlan ) )
			{
				System.Diagnostics.Process.Start( "http://localhost:17575/" );
			}
		}

		private bool IsPlanSufficient( MealPlan mealPlan )
		{
			return mealPlan.MealPlanDays.Any( d => d.Day.Date == DateTime.Now.Date ) &&
				mealPlan.MealPlanDays.Any( d => d.Day.Date == DateTime.Now.Date.AddDays( 1 ) ) &&
				mealPlan.MealPlanDays.Any( d => d.Day.Date == DateTime.Now.Date.AddDays( 2 ) );
		}

		private Timer _updateTimer;
	}
}
