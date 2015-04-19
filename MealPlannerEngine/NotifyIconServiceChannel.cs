using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace MealPlannerEngine
{
	class NotifyIconServiceChannel
	{
		public NotifyIconServiceChannel()
		{
			_baseAddress = new Uri( "http://localhost:17576" );
		}

		public void SendMealPlanDaysNeeded(int daysNeeded)
		{
			using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( _baseAddress ) )
			{
				INotifyIconService channel = cf.CreateChannel();

				channel.NotifyPlanDaysNeeded( daysNeeded );
			}
		}

		public void SendShoppingNeeded( int daysLeft )
		{
			using ( WebChannelFactory<INotifyIconService> cf = new WebChannelFactory<INotifyIconService>( _baseAddress ) )
			{
				INotifyIconService channel = cf.CreateChannel();

				channel.NotifyShoppingNeeded( daysLeft );
			}
		}

		private Uri _baseAddress;
	}
}
