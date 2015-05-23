using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace MealPlannerEngine
{
	class NotifyIconServiceChannel
	{
		public NotifyIconServiceChannel( EventLog eventLog )
		{
			_eventLog = eventLog;
		}

		public void SendMealPlanDaysNeeded( int daysNeeded )
		{
			var serviceConfig = new Serializer().GetConfiguration().NotifyIconService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( serviceAddress ) )
				{
					INotifyIconService channel = cf.CreateChannel();

					channel.NotifyPlanDaysNeeded( daysNeeded );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SendMealPlanDaysNeeded with daysNeeded '{0}' sending to {1}\n{2}", daysNeeded, serviceAddress, e.Message ), EventLogEntryType.Error, 6 );
			}
		}

		public void SendShoppingNeeded( int daysLeft )
		{
			var serviceConfig = new Serializer().GetConfiguration().NotifyIconService;
			var serviceAddress = String.Format( "http://{0}:{1}", serviceConfig.HostName, serviceConfig.Port );

			try
			{
				using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( serviceAddress ) )
				{
					INotifyIconService channel = cf.CreateChannel();

					channel.NotifyShoppingNeeded( daysLeft );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SendShoppingNeeded with daysLeft '{0}' sending to {1}\n{2}", daysLeft, serviceAddress, e.Message ), EventLogEntryType.Error, 7 );
			}
		}

		private EventLog _eventLog;
	}
}
