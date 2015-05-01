(function() {
	angular.module("MealPlanner")
	.directive("calendarMealPlan", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/CalendarMealPlan.html',
			scope: {
				selectedDay: "="
			},
			controller: ['$scope', function($scope) {
				$scope.Today = new Date();
				$scope.calendarDays = [];

				var dayOfWeek = $scope.Today.getDay();

				$scope.startDate = $scope.Today;
				for (var i = 0; i < dayOfWeek; ++i) {
					$scope.startDate = subtractDay($scope.startDate);
				}
				$scope.lastDate = $scope.startDate;
				for (var i = 0; i < 21; ++i) {
					$scope.lastDate = addDay($scope.lastDate);
				}

				$scope.SelectDay = function(calendarDay) {
					$scope.$emit('SelectDay', { SelectedDate: calendarDay.Date });
				};

				$scope.IsDaySelected = function(calendarDay) {
					if (!$scope.selectedDay) return false;
					return isPlanDayEqualToDate($scope.selectedDay, calendarDay.Date);
				};

				$scope.GetHeaderString = function() {
					var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

					if ($scope.startDate.getMonth() == $scope.lastDate.getMonth()) {
						return monthNames[$scope.startDate.getMonth()];
					}
					else {
						return monthNames[$scope.startDate.getMonth()] + " - " + monthNames[$scope.lastDate.getMonth()];
					}
				};

				$scope.GetDayPlanClass = function(calendarDay) {
					var renderType = "unplanned";
					if (!calendarDay) return renderType;

					if (calendarDay.ShoppedOn) {
						renderType = "shopped-on";
					}
					else if (calendarDay.ShoppedFor) {
						renderType = "shopped-for";
					}
					else if (calendarDay.Planned) {
						renderType = "planned";
					}

					return renderType;
				};

				function populateCalendar() {
					var currentDate = $scope.startDate;
					$scope.Weeks = [];
					for (var i = 0; i < 3; ++i) {
						$scope.Weeks.push([]);
						var week = $scope.Weeks[i];

						for (var j = 0; j < 7; ++j) {
							var planDay = findPlanDay(currentDate);

							var breakfast = !!planDay.Breakfast ? planDay.Breakfast.Name : 'None';
							var lunch = !!planDay.Lunch ? planDay.Lunch.Name : 'None';
							var dinner = !!planDay.Dinner ? planDay.Dinner.Name : 'None';
							var planned = !!planDay.Breakfast && !!planDay.Lunch && !!planDay.Dinner;
							var shoppedFor = planDay.ShoppedFor;
							var shoppedOn = planDay.ShoppedOn;

							week.push({
								Date: currentDate,
								Number: currentDate.getDate(),
								BreakfastName: breakfast,
								LunchName: lunch,
								DinnerName: dinner,
								Planned: planned,
								ShoppedFor: shoppedFor,
								ShoppedOn: shoppedOn
							});

							currentDate = addDay(currentDate);
						}
					}
				}
				populateCalendar();

				$scope.$on('CalendarUpdated', function(event, args) {
					$scope.calendarDays = args.CalendarDays;
					populateCalendar();
				});

				function requestCalendarUpdate() {
					$scope.$emit('SetCalendarDates', {
						FirstDate: $scope.startDate,
						LastDate: $scope.lastDate
					});
				}
				requestCalendarUpdate();
				$scope.$on('MealPlanUpdated', requestCalendarUpdate);

				$scope.$emit('SelectDay', { SelectedDate: new Date() });

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
					for (var i = 0; i < $scope.calendarDays.length; ++i) {
						var mealPlanDay = $scope.calendarDays[i];
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
