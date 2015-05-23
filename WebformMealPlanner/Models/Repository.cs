﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using MealPlanner.Library;

namespace WebformMealPlanner.Models
{
	public class MealPlannerRepository
	{
		public MealPlannerRepository()
		{
			_eventLog = new EventLog();
			if ( !EventLog.SourceExists( "Web Source" ) )
			{
				EventLog.CreateEventSource( "Web Source", "MealPlannerLog" );
			}
			_eventLog.Source = "Web Source";
			_eventLog.Log = "MealPlannerLog";

			_currentMealPlan = new MealPlannerEngineServiceChannel( _eventLog ).GetMealPlan();
			_currentMealOptions = new MealPlannerEngineServiceChannel( _eventLog ).GetMealOptions();
			_currentConfiguration = new MealPlannerEngineServiceChannel( _eventLog ).GetConfiguration();

			_currentMealPlan.MealPlanDays.Sort( new MealPlanDaysByDateComparer() );
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
			RemoveMealPlanDayFromMealPlan( day.Day );

			var mealPlanDay = new MealPlanDay();
			mealPlanDay.Breakfast = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.BreakfastName ) ).FirstOrDefault();
			mealPlanDay.Lunch = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.LunchName ) ).FirstOrDefault();
			mealPlanDay.Dinner = _currentMealOptions.Where( mo => String.Equals( mo.Name, day.DinnerName ) ).FirstOrDefault();
			mealPlanDay.Day = day.Day.ToDateTime();

			_currentMealPlan.MealPlanDays.Add( mealPlanDay );

			_currentMealPlan.MealPlanDays.Sort( new MealPlanDaysByDateComparer() );

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

		public MealPlanViewModel RemoveMealPlanDay( JavascriptDateTime day )
		{
			RemoveMealPlanDayFromMealPlan( day );

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
					Day = new JavascriptDateTime( d.Day )
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

		private void RemoveMealPlanDayFromMealPlan( JavascriptDateTime day )
		{
			var mealPlanDay = _currentMealPlan.MealPlanDays.Where( d => day.ToDateTime() == d.Day ).FirstOrDefault();

			if ( mealPlanDay != null )
			{
				_currentMealPlan.MealPlanDays.Remove( mealPlanDay );
				new Serializer().SetMealPlan( _currentMealPlan );
			}
		}

		private MealPlan _currentMealPlan;
		private List<MealOption> _currentMealOptions;
		private MealPlannerConfiguration _currentConfiguration;
		private string _processAddress;
		private EventLog _eventLog;
	}
}