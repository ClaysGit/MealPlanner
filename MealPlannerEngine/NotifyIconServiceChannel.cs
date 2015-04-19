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
			_baseAddress = new Uri( "http://localhost:17576" );
			_eventLog = eventLog;
		}

		public void SendMealPlanDaysNeeded( int daysNeeded )
		{
			try
			{
				using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( _baseAddress ) )
				{
					INotifyIconService channel = cf.CreateChannel();

					channel.NotifyPlanDaysNeeded( daysNeeded );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SendMealPlanDaysNeeded with daysNeeded '{0}'\n{1}", daysNeeded, e.Message ), EventLogEntryType.Error, 4 );
			}
		}

		public void SendShoppingNeeded( int daysLeft )
		{
			try
			{
				using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( _baseAddress ) )
				{
					INotifyIconService channel = cf.CreateChannel();

					channel.NotifyShoppingNeeded( daysLeft );
				}
			}
			catch ( Exception e )
			{
				_eventLog.WriteEntry( String.Format( "Exception in SendShoppingNeeded with daysLeft '{0}'\n{1}", daysLeft, e.Message ), EventLogEntryType.Error, 5 );
			}
		}

		private Uri _baseAddress;
		private EventLog _eventLog;
	}
}
