using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MealPlanner.Library
{
	[ServiceContract]
	public interface INotifyIconService
	{
		[OperationContract]
		void NotifyPlanDaysNeeded( int daysNeeded );

		[OperationContract]
		void NotifyShoppingNeeded( int daysLeft );
	}
}
