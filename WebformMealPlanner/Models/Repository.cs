using System;
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

            _defaultEngineConfiguration = new Serializer().GetConfiguration();

            _currentMealPlan.MealPlanDays.Sort(new MealPlanDaysByDateComparer());
        }

        public DefaultEngineViewPersistModel GetDefaultEngineViewModel()
        {
            return MealPlannerConfigurationToDefaultEngineViewMOdel(_defaultEngineConfiguration);
        }

        public IndexViewModel GetIndexViewModel()
        {
            return new IndexViewModel
            {
                MealPlan = MealPlanToViewModel(_currentMealPlan),
                MealOptions = MealOptionsToViewModel(_currentMealOptions),
                MealPlannerConfiguration = MealPlannerConfigurationToViewModel(_currentConfiguration)
            };
        }

        public MealPlanViewModel AddMealPlanDay(MealPlanDayPersistModel day)
        {
            RemoveMealPlanDayFromMealPlan(day.Day);

            var mealPlanDay = new MealPlanDay();
            mealPlanDay.Breakfast = _currentMealOptions.Where(mo => String.Equals(mo.Name, day.BreakfastName)).FirstOrDefault();
            mealPlanDay.Lunch = _currentMealOptions.Where(mo => String.Equals(mo.Name, day.LunchName)).FirstOrDefault();
            mealPlanDay.Dinner = _currentMealOptions.Where(mo => String.Equals(mo.Name, day.DinnerName)).FirstOrDefault();
            mealPlanDay.Day = day.Day.ToDateTime();

            _currentMealPlan.MealPlanDays.Add(mealPlanDay);

            _currentMealPlan.MealPlanDays.Sort(new MealPlanDaysByDateComparer());

            new Serializer().SetMealPlan(_currentMealPlan);

            return MealPlanToViewModel(_currentMealPlan);
        }

        public MealOptionsViewModel AddMealOption(MealOptionViewPersistModel mealOption)
        {
            mealOption.KeyIngredients = mealOption.KeyIngredients ?? new string[0];

            var newMealOption = new MealOption();
            newMealOption.Name = mealOption.Name;
            newMealOption.KeyIngredients = mealOption.KeyIngredients.ToList();

            _currentMealOptions.Add(newMealOption);


            return MealOptionsToViewModel(_currentMealOptions);
        }

        public MealPlanViewModel RemoveMealPlanDay(JavascriptDateTime day)
        {
            RemoveMealPlanDayFromMealPlan(day);

            return MealPlanToViewModel(_currentMealPlan);
        }

        public MealOptionsViewModel RemoveMealOption(string name)
        {
            var mealOption = _currentMealOptions.Where(o => o.Name == name).FirstOrDefault();

            if (mealOption != null)
            {
                _currentMealOptions.Remove(mealOption);
                new Serializer().SetMealOptions(_currentMealOptions);
            }

            return MealOptionsToViewModel(_currentMealOptions);
        }

        public MealPlannerConfigurationViewPersistModel SetMealPlannerConfiguration(MealPlannerConfigurationViewPersistModel persistModel)
        {
            _currentConfiguration.EngineService = _currentConfiguration.EngineService ?? new MealPlannerServiceConfiguration();
            _currentConfiguration.WebService = _currentConfiguration.WebService ?? new MealPlannerServiceConfiguration();
            _currentConfiguration.NotifyIconService = _currentConfiguration.NotifyIconService ?? new MealPlannerServiceConfiguration();

            _currentConfiguration.EngineService.HostName = persistModel.EngineServiceHostName ?? _currentConfiguration.EngineService.HostName;
            _currentConfiguration.EngineService.Port = persistModel.EngineServicePort ?? _currentConfiguration.EngineService.Port;

            _currentConfiguration.WebService.HostName = persistModel.WebServiceHostName ?? _currentConfiguration.WebService.HostName;
            _currentConfiguration.WebService.Port = persistModel.WebServicePort ?? _currentConfiguration.WebService.Port;

            _currentConfiguration.NotifyIconService.HostName = persistModel.NotifyIconServiceHostName ?? _currentConfiguration.NotifyIconService.HostName;
            _currentConfiguration.NotifyIconService.Port = persistModel.NotifyIconServicePort ?? _currentConfiguration.NotifyIconService.Port;

            _currentConfiguration.DaysToPlan = persistModel.DaysToPlan != -1 ? persistModel.DaysToPlan : _currentConfiguration.DaysToPlan;
            _currentConfiguration.ShoppingDaysNeeded = persistModel.ShoppingDaysNeeded != -1 ? persistModel.ShoppingDaysNeeded : _currentConfiguration.ShoppingDaysNeeded;

            new Serializer().SetConfiguration(_currentConfiguration);

            return MealPlannerConfigurationToViewModel(_currentConfiguration);
        }

        public void SetDefaultEngineConfiguration(DefaultEngineViewPersistModel configuration)
        {
            _defaultEngineConfiguration.EngineService.HostName = configuration.HostName;
            _defaultEngineConfiguration.EngineService.Port = configuration.Port;
            new Serializer().SetConfiguration(_defaultEngineConfiguration);
        }

        private MealPlanViewModel MealPlanToViewModel(MealPlan mealPlan)
        {
            var viewModel = new MealPlanViewModel();

            viewModel.MealPlanDays = mealPlan.MealPlanDays.Select(d =>
            {
                return new MealPlanDayViewModel()
                {
                    Breakfast = MealOptionToViewModel(d.Breakfast),
                    Lunch = MealOptionToViewModel(d.Lunch),
                    Dinner = MealOptionToViewModel(d.Dinner),
                    Day = new JavascriptDateTime(d.Day)
                };
            }).ToArray();

            return viewModel;
        }

        private MealOptionsViewModel MealOptionsToViewModel(List<MealOption> mealOptions)
        {
            var viewModel = new MealOptionsViewModel();

            viewModel.MealOptions = mealOptions.Select(MealOptionToViewModel).ToArray();

            return viewModel;
        }

        private MealOptionViewPersistModel MealOptionToViewModel(MealOption option)
        {
            if (option == null)
            {
                return new MealOptionViewPersistModel
                {
                    Name = "<No name>",
                    KeyIngredients = new string[0]
                };
            }

            return new MealOptionViewPersistModel
            {
                Name = option.Name,
                KeyIngredients = option.KeyIngredients.ToArray()
            };
        }

        private MealPlannerConfigurationViewPersistModel MealPlannerConfigurationToViewModel(MealPlannerConfiguration configuration)
        {
            return new MealPlannerConfigurationViewPersistModel
            {
                EngineServiceHostName = configuration.EngineService.HostName,
                EngineServicePort = configuration.EngineService.Port,

                WebServiceHostName = configuration.WebService.HostName,
                WebServicePort = configuration.WebService.Port,

                NotifyIconServiceHostName = configuration.NotifyIconService.HostName,
                NotifyIconServicePort = configuration.NotifyIconService.Port,

                DaysToPlan = configuration.DaysToPlan,
                ShoppingDaysNeeded = configuration.ShoppingDaysNeeded
            };
        }

        private DefaultEngineViewPersistModel MealPlannerConfigurationToDefaultEngineViewMOdel(MealPlannerConfiguration configuration)
        {
            return new DefaultEngineViewPersistModel
            {
                HostName = configuration.EngineService.HostName,
                Port = configuration.EngineService.Port
            };
        }

        private void RemoveMealPlanDayFromMealPlan(JavascriptDateTime day)
        {
            var mealPlanDay = _currentMealPlan.MealPlanDays.Where(d => day.ToDateTime() == d.Day).FirstOrDefault();

            if (mealPlanDay != null)
            {
                _currentMealPlan.MealPlanDays.Remove(mealPlanDay);
                new Serializer().SetMealPlan(_currentMealPlan);
            }
        }

        private MealPlan _currentMealPlan;
        private List<MealOption> _currentMealOptions;
        private MealPlannerConfiguration _currentConfiguration;
        private MealPlannerConfiguration _defaultEngineConfiguration;
		private EventLog _eventLog;
    }
}
