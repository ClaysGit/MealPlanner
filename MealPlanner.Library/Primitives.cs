using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlanner.Library
{
	public class MealOption
	{
		public MealOption()
		{
			KeyIngredients = new List<string>();
		}

		public string Name { get; set; }
		public List<string> KeyIngredients { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}

	public class MealPlan
	{
		public MealPlan()
		{
			MealPlanDays = new List<MealPlanDay>();
		}

		public List<MealPlanDay> MealPlanDays { get; set; }
	}

	public class MealPlanDay
	{
		public MealOption Breakfast { get; set; }
		public MealOption Lunch { get; set; }
		public MealOption Dinner { get; set; }

		public DateTime Day { get; set; }

		public bool ShoppedFor { get; set; }
		public bool ShoppedOn { get; set; }

		public override string ToString()
		{
			var mealsString = "No Meals Planned";
			var mealsList = new List<MealOption>();
			if ( Breakfast != null ) mealsList.Add( Breakfast );
			if ( Lunch != null ) mealsList.Add( Lunch );
			if ( Dinner != null ) mealsList.Add( Dinner );
			if ( mealsList.Any() )
			{
				mealsString = String.Join( ", ", mealsList );
			}

			if ( Day.Date == DateTime.Now.Date )
			{
				return String.Format( "Today:\t\t{0}", mealsString );
			}
			else if ( Day.Date == DateTime.Now.Date + new TimeSpan( 1, 0, 0, 0 ) )
			{
				return String.Format( "Tomorrow:\t{0}", mealsString );
			}
			else if ( Day.Date < DateTime.Now.Date + new TimeSpan( 7, 0, 0, 0 ) )
			{
				return String.Format( "{0}:\t\t{1}", Day.ToString( "ddd" ), mealsString );
			}

			return String.Format( "{0}:\t\t{1}", Day.ToString( "M/d" ), mealsString );
		}
	}

	public class MealPlannerConfiguration
	{
		public MealPlannerConfiguration()
		{
			EngineService = new MealPlannerServiceConfiguration
			{
				HostName = "localhost",
				Port = "17575"
			};
			WebService = new MealPlannerServiceConfiguration
			{
				HostName = "localhost",
				Port = "17576"
			};
			NotifyIconService = new MealPlannerServiceConfiguration
			{
				HostName = "localhost",
				Port = "17577"
			};

			DaysToPlan = 5;
			ShoppingDaysNeeded = 2;
		}

		public MealPlannerServiceConfiguration NotifyIconService { get; set; }
		public MealPlannerServiceConfiguration WebService { get; set; }
		public MealPlannerServiceConfiguration EngineService { get; set; }

		public int DaysToPlan { get; set; }
		public int ShoppingDaysNeeded { get; set; }
	}

	public class MealPlannerServiceConfiguration
	{
		public string Port { get; set; }
		public string HostName { get; set; }
	}
}
