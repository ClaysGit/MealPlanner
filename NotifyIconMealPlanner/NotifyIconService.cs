using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace NotifyIconMealPlanner
{
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	class NotifyIconService : INotifyIconService
	{
		public NotifyIconService( IMealPlannerNotifyIcon notifyIcon )
		{
			_notifyIcon = notifyIcon;
		}

		public void NotifyPlanDaysNeeded( int daysNeeded )
		{
			_notifyIcon.ShowPopupForPlanDaysNeeded( daysNeeded );
		}

		public void NotifyShoppingNeeded( int daysLeft )
		{
			_notifyIcon.ShowPopupForShoppingNeeded( daysLeft );
		}

		private IMealPlannerNotifyIcon _notifyIcon;
	}
}
