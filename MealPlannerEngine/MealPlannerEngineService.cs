using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace MealPlannerEngine
{
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	public class MealPlannerEngineService : IMealPlannerEngineService
	{
		public MealPlan GetMealPlan()
		{
			return new Serializer().GetMealPlan();
		}

		public List<MealOption> GetMealOptions()
		{
			return new Serializer().GetMealOptions();
		}

		public MealPlannerConfiguration GetConfiguration()
		{
			return new Serializer().GetConfiguration();
		}

		public void SetMealPlan( MealPlan mealPlan )
		{
			new Serializer().SetMealPlan( mealPlan );
		}

		public void SetMealOptions( List<MealOption> mealOptions )
		{
			new Serializer().SetMealOptions( mealOptions );
		}

		public void SetConfiguration( MealPlannerConfiguration configuration )
		{
			new Serializer().SetConfiguration( configuration );
		}
	}
}
