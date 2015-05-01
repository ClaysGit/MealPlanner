(function() {
	angular.module("MealPlanner")
	.directive("calendarMealPlan", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/CalendarMealPlan.html',
			scope: {
				data: "=",
				selectedDay: "="
			},
			controller: ['$scope', function($scope) {
				$scope.Today = new Date();

				var dayOfWeek = $scope.Today.getDay();

				var startDate = $scope.Today;
				for (var i = 0; i < dayOfWeek; ++i) {
					startDate = subtractDay(startDate);
				}

				$scope.SelectDay = function(calendarDay) {
					$scope.selectedDay = calendarDay.PlanDay;
				};

				$scope.IsDaySelected = function(calendarDay) {
					if (!$scope.selectedDay) return false;
					return isPlanDayEqualToDate($scope.selectedDay, calendarDay.Date);
				};

				$scope.GetHeaderString = function() {
					var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

					var lastDay = $scope.Weeks[2][6].Date;

					if (startDate.getMonth() == lastDay.getMonth()) {
						return monthNames[startDate.getMonth()];
					}
					else {
						return monthNames[startDate.getMonth()] + " - " + monthNames[lastDay.getMonth()];
					}
				};

				function populateCalendar() {
					var currentDate = startDate;
					$scope.Weeks = [];
					for (var i = 0; i < 3; ++i) {
						$scope.Weeks.push([]);
						var week = $scope.Weeks[i];

						for (var j = 0; j < 7; ++j) {
							var planDay = findPlanDay(currentDate);
							var renderType = "unplanned";
							if (planDay.ShoppedOn) {
								renderType = "shopped-on";
							}
							else if (planDay.ShoppedFor) {
								renderType = "shopped-for";
							}
							else if (planDay.Breakfast != null &&
								planDay.Lunch != null &&
								planDay.Dinner != null) {
								renderType = "planned";
							}

							week.push({
								Date: currentDate,
								Number: currentDate.getDate(),
								ClassName: renderType,
								PlanDay: planDay
							});

							currentDate = addDay(currentDate);
						}
					}
				}
				populateCalendar();

				$scope.$on('MealPlanUpdated', populateCalendar);

				function addDay(date) {
					var dateTime = date.getTime();
					//1 day has 24 hours has 60 minutes has 60 seconds has 1000 milliseconds
					var tomorrowTime = dateTime + (1 * 24 * 60 * 60 * 1000);
					return new Date(tomorrowTime);
				}

				function subtractDay(date) {
					var dateTime = date.getTime();
					//1 day has 24 hours has 60 minutes has 60 seconds has 1000 milliseconds
					var yesterdayTime = dateTime - (1 * 24 * 60 * 60 * 1000);
					return new Date(yesterdayTime);
				}

				function findPlanDay(date) {
					for (var i = 0; i < $scope.data.MealPlanDays.length; ++i) {
						var mealPlanDay = $scope.data.MealPlanDays[i];
						if (isPlanDayEqualToDate(mealPlanDay, date)) {
							return mealPlanDay;
						}
					}

					return {
						Day: {
							DayOfMonth: date.getDate(),
							Month: date.getMonth(),
							FullYear: date.getFullYear()
						},
						Breakfast: null,
						Lunch: null,
						Dinner: null
					};
				}

				function isPlanDayEqualToDate(planDay, date) {
					if (!planDay || !planDay.Day) return false;
					return planDay.Day.FullYear == date.getFullYear() &&
							planDay.Day.Month == date.getMonth() &&
							planDay.Day.DayOfMonth == date.getDate();
				}
			}]
		};
	});
})();
