using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace WebformMealPlanner.Models
{
	class MealPlannerEngineServiceChannel
	{
		public MealPlannerEngineServiceChannel( EventLog eventLog )
		{
			_eventLog = eventLog;
		}

		public MealPlan GetMealPlan()
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					return channel.GetMealPlan();
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in GetMealPlan sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				return new Serializer().GetMealPlan();
			}
		}

		public List<MealOption> GetMealOptions()
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					return channel.GetMealOptions();
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in GetMealOptions sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				return new Serializer().GetMealOptions();
			}
		}

		public MealPlannerConfiguration GetConfiguration()
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					return channel.GetConfiguration();
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in GetConfiguration sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				return new Serializer().GetConfiguration();
			}
		}

		public void SetMealPlan( MealPlan mealPlan )
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					channel.SetMealPlan( mealPlan );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SetMealPlan sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				new Serializer().SetMealPlan( mealPlan );
			}
		}

		public void SetMealOptions( List<MealOption> mealOptions )
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					channel.SetMealOptions( mealOptions );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SetMealOptions sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				new Serializer().SetMealOptions( mealOptions );
			}
		}

		public void SetConfiguration( MealPlannerConfiguration configuration )
		{
			var serviceConfig = new Serializer().GetConfiguration().EngineService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( serviceAddress ) )
				{
					IMealPlannerEngineService channel = cf.CreateChannel();

					channel.SetConfiguration( configuration );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SetConfiguration sending to {1}\n{2}", serviceAddress, e.Message ), EventLogEntryType.Error, 0 );
				new Serializer().SetConfiguration( configuration );
			}
		}

		private EventLog _eventLog;
	}
}
