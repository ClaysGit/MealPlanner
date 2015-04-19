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
		public MealOptionViewPersistModel Breakfast;
		public MealOptionViewPersistModel Lunch;
		public MealOptionViewPersistModel Dinner;

		public string Day;
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
	}
}