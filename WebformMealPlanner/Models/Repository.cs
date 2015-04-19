using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MealPlanner.Library;

namespace WebformMealPlanner.Models
{
	public class MealPlannerRepository
	{
		public MealPlannerRepository()
		{
			_currentMealPlan = new Serializer().GetMealPlan();
			_currentMealOptions = new Serializer().GetMealOptions();
		}

		public MealPlanViewModel GetMealPlanViewModel()
		{
			return MealPlanToViewModel( _currentMealPlan );
		}

		public MealOptionsViewModel GetMealOptionsViewModel()
		{
			return MealOptionsToViewModel( _currentMealOptions );
		}

		public MealPlanViewModel AddMealPlanDay( MealPlanDayPersistModel day )
		{
			var mealPlanDay = new MealPlanDay();
			mealPlanDay.Breakfast = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.BreakfastName ) ).FirstOrDefault();
			mealPlanDay.Lunch = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.LunchName ) ).FirstOrDefault();
			mealPlanDay.Dinner = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.DinnerName ) ).FirstOrDefault();

			if ( _currentMealPlan.MealPlanDays.Any() )
			{
				mealPlanDay.Day = _currentMealPlan.MealPlanDays.Select( d => d.Day ).Max() + TimeSpan.FromDays( 1 );
			}
			else
			{
				mealPlanDay.Day = DateTime.Now;
			}

			_currentMealPlan.MealPlanDays.Add( mealPlanDay );

			new Serializer().SetMealPlan( _currentMealPlan );

			return MealPlanToViewModel( _currentMealPlan );
		}

		public MealOptionsViewModel AddMealOption( MealOptionViewPersistModel mealOption )
		{
			mealOption.KeyIngredients = mealOption.KeyIngredients ?? new string[ 0 ];

			var newMealOption = new MealOption();
			newMealOption.Name = mealOption.Name;
			newMealOption.KeyIngredients = mealOption.KeyIngredients.ToList();

			_currentMealOptions.Add( newMealOption );

			new Serializer().SetMealOptions( _currentMealOptions );

			return MealOptionsToViewModel( _currentMealOptions );
		}

		public MealPlanViewModel RemoveMealPlanDay( string day )
		{
			var mealPlanDay = _currentMealPlan.MealPlanDays.Where( d => d.Day.ToString( _dateFormat ) == day ).FirstOrDefault();

			if ( mealPlanDay != null )
			{
				_currentMealPlan.MealPlanDays.Remove( mealPlanDay );
				new Serializer().SetMealPlan( _currentMealPlan );
			}

			return MealPlanToViewModel( _currentMealPlan );
		}

		public MealOptionsViewModel RemoveMealOption( string name )
		{
			var mealOption = _currentMealOptions.Where( o => o.Name == name ).FirstOrDefault();

			if ( mealOption != null )
			{
				_currentMealOptions.Remove( mealOption );
				new Serializer().SetMealOptions( _currentMealOptions );
			}

			return MealOptionsToViewModel( _currentMealOptions );
		}

		private MealPlanViewModel MealPlanToViewModel( MealPlan mealPlan )
		{
			var viewModel = new MealPlanViewModel();

			viewModel.MealPlanDays = mealPlan.MealPlanDays.Select( d =>
			{
				return new MealPlanDayViewModel()
				{
					Breakfast = MealOptionToViewModel( d.Breakfast ),
					Lunch = MealOptionToViewModel( d.Lunch ),
					Dinner = MealOptionToViewModel( d.Dinner ),
					Day = d.Day.ToString( _dateFormat )
				};
			} ).ToArray();

			return viewModel;
		}

		private MealOptionsViewModel MealOptionsToViewModel( List<MealOption> mealOptions )
		{
			var viewModel = new MealOptionsViewModel();

			viewModel.MealOptions = mealOptions.Select( MealOptionToViewModel ).ToArray();

			return viewModel;
		}

		private MealOptionViewPersistModel MealOptionToViewModel( MealOption option )
		{
			if ( option == null )
			{
				return new MealOptionViewPersistModel
				{
					Name = "<No name>",
					KeyIngredients = new string[ 0 ]
				};
			}

			return new MealOptionViewPersistModel
			{
				Name = option.Name,
				KeyIngredients = option.KeyIngredients.ToArray()
			};
		}

		private MealPlan _currentMealPlan;
		private List<MealOption> _currentMealOptions;
		private const string _dateFormat = "r";
	}
}