using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

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

	[ServiceContract]
	public interface IMealPlanDataProvider
	{
		[OperationContract]
		MealPlan GetMealPlan();

		[OperationContract]
		List<MealOption> GetMealOptions();

		[OperationContract]
		MealPlannerConfiguration GetConfiguration();

		[OperationContract]
		void SetMealPlan( MealPlan mealPlan );

		[OperationContract]
		void SetMealOptions( List<MealOption> mealOptions );

		[OperationContract]
		void SetConfiguration( MealPlannerConfiguration configuration );
	}

	[ServiceContract]
	public interface IMealPlannerEngineService : IMealPlanDataProvider
	{
		//Empty
	}
}
