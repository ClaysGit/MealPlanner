(function() {
	angular.module("MealPlanner", ["ng"])
		.controller("MealPlannerController", ["$scope", "$http", function($scope, $http) {
			$scope.mealPlan = {};
			$scope.mealOptions = { MealOptions: [] };
			$scope.mealPlannerConfiguration = {};
			$scope.mode = 'MealPlan'; //MealPlan, MealOptions, Config
			$scope.calendarDays = [];
			$scope.selectedDay = null;

			$scope.initialized = false;

			$scope.$on('AddMealPlanDay', function(event, args) {
				$http.post('MealPlanner/AddMealPlanDay', {
					day: {
						BreakfastName: args.Breakfast.Name,
						LunchName: args.Lunch.Name,
						DinnerName: args.Dinner.Name,
						Day: args.Day
					}
				})
				.success(function(response) {
					$scope.mealPlan = response;
					$scope.$broadcast('MealPlanUpdated');
				});
			});

			$scope.$on('AddMealOption', function(event, args) {
				$http.post('MealPlanner/AddMealOption', {
					mealOption: {
						Name: args.Name,
						KeyIngredients: args.KeyIngredients
					}
				})
				.success(function(response) {
					$scope.mealOptions = response;
				});
			});

			$scope.$on('RemoveMealPlanDay', function(event, args) {
				$http.post('MealPlanner/RemoveMealPlanDay', {
					day: args.Day
				})
				.success(function(response) {
					$scope.mealPlan = response;
					$scope.$broadcast('MealPlanUpdated');
				});
			});

			$scope.$on('RemoveMealOption', function(event, args) {
				$http.post('MealPlanner/RemoveMealOption', {
					name: args.Name
				})
				.success(function(response) {
					$scope.mealOptions = response;
				});
			});

			$scope.$on('SetMealPlannerConfiguration', function (event, args) {
			    $http.post('MealPlanner/SetMealPlannerConfiguration', {
                    configuration: args
			    })
                .success(function (response) {
                    $scope.Initialize();
                });
			});

			$scope.$on('SetDefaultEngineConfiguration', function (event, args) {
			    $http.post('MealPlanner/SetDefaultEngineConfiguration', {
                    configuration: args
			    })
                .success(function (response) {

                })
			});

			$scope.$on('SetCalendarDates', function(event, args) {
				var firstDate = args.FirstDate;
				var lastDate = args.LastDate;

				$scope.calendarDays = [];
				for (var i = 0; i < $scope.mealPlan.MealPlanDays.length; ++i) {
					var planDay = $scope.mealPlan.MealPlanDays[i];
					var date = new Date(planDay.Day.FullYear, planDay.Day.Month, planDay.Day.DayOfMonth);

					if (simpleDateLTE(firstDate, date) && simpleDateLTE(date, lastDate)) {
						$scope.calendarDays.push(planDay);
					}
				}

				$scope.$broadcast('CalendarUpdated', { CalendarDays: $scope.calendarDays });
			});

			$scope.$on('SelectDay', function(event, args) {
				var date = args.SelectedDate;

				$scope.selectedDay = {
					Day: {
						FullYear: date.getFullYear(),
						Month: date.getMonth(),
						DayOfMonth: date.getDate()
					}
				};
				for (var i = 0; i < $scope.mealPlan.MealPlanDays.length; ++i) {
					var planDay = $scope.mealPlan.MealPlanDays[i];

					if (planDay.Day.FullYear == date.getFullYear()
						&& planDay.Day.Month == date.getMonth()
						&& planDay.Day.DayOfMonth == date.getDate()) {
						$scope.selectedDay = planDay;
						return;
					}
				}
			});

			$scope.Initialize = function () {
			    $http.post('MealPlanner/GetIndexViewModel')
                .success(function (response) {
                    $scope.mealPlan = response.MealPlan;
                    $scope.mealOptions = response.MealOptions;
                    $scope.mealPlannerConfiguration = response.mealPlannerConfiguration;
                    $scope.initialized = true;

                    $scope.$broadcast('MealPlanUpdated');
                });
			};

			$scope.Initialize();

			function simpleDateLTE(date1, date2) {
				if (date1.getFullYear() < date2.getFullYear())	return true;
				else if (date1.getFullYear() > date2.getFullYear()) return false;

				if (date1.getMonth() < date2.getMonth()) return true;
				else if (date1.getMonth() > date2.getMonth()) return false;

				if (date1.getDate() < date2.getDate()) return true;
				else if (date1.getDate() > date2.getDate()) return false;

				return true;
			}
		}]);
})();
