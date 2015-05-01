using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MealPlanner.Library;

namespace WebformMealPlanner.Models
{
	public class MealPlannerModel
	{
		public MealPlanViewModel MealPlan { get; set; }
		public MealOptionsViewModel MealOptions { get; set; }
	}

	public class MealPlanViewModel
	{
		public MealPlanDayViewModel[] MealPlanDays { get; set; }
	}

	public class MealPlanDayViewModel
	{
		public MealOptionViewPersistModel Breakfast { get; set; }
		public MealOptionViewPersistModel Lunch { get; set; }
		public MealOptionViewPersistModel Dinner { get; set; }

		public bool ShoppedFor { get; set; }
		public bool ShoppedOn { get; set; }

		public JavascriptDateTime Day { get; set; }
	}

	public class MealOptionsViewModel
	{
		public MealOptionViewPersistModel[] MealOptions { get; set; }
	}

	public class MealOptionViewPersistModel
	{
		public string Name { get; set; }

		public string[] KeyIngredients { get; set; }
	}

	public class MealPlanDayPersistModel
	{
		public string BreakfastName { get; set; }
		public string LunchName { get; set; }
		public string DinnerName { get; set; }
		public JavascriptDateTime Day { get; set; }
	}

	public class JavascriptDateTime
	{
		public JavascriptDateTime()
			: this( DateTime.Now )
		{

		}

		public JavascriptDateTime( DateTime fromDate )
		{
			FullYear = fromDate.Year;
			Month = fromDate.Month - 1;
			DayOfMonth = fromDate.Day;
		}

		public int FullYear { get; set; }
		public int Month { get; set; }
		public int DayOfMonth { get; set; }

		public DateTime ToDateTime()
		{
			return new DateTime( FullYear, Month + 1, DayOfMonth );
		}
	}

	public class MealPlanDaysByDateComparer: IComparer<MealPlanDay>
	{
		public int Compare(MealPlanDay x, MealPlanDay y)
		{
			if ( x.Day < y.Day ) return -1;
			if ( x.Day > y.Day ) return 1;
			return 0;
		}
	}
}