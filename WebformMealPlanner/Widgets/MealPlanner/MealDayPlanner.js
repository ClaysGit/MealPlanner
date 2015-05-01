(function() {
	angular.module("MealPlanner")
	.directive("mealDayPlanner", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealDayPlanner.html',
			scope: {
				data: "=",
				mealOptions: "="
			},
			controller: ['$scope', function($scope) {
				$scope.CurrentMeal = 'Breakfast';

				$scope.Delete = function(data) {
					$scope.$emit('RemoveMealPlanDay', data);
					delete $scope.data.Breakfast;
					delete $scope.data.Lunch;
					delete $scope.data.Dinner;
					$scope.ResetEditable();
				};

				$scope.Save = function(data) {
					$scope.$emit('AddMealPlanDay', data);
				};

				$scope.ResetEditable = function() {
					var choice = {};
					if ($scope.mealOptions && $scope.mealOptions.MealOptions.length > 0) {
						choice = $scope.mealOptions.MealOptions[0];
					}

					var date = new Date();

					var breakfast = choice;
					var lunch = choice;
					var dinner = choice;
					var day = {
						FullYear: date.getFullYear(),
						Month: date.getMonth(),
						DayOfMonth: date.getDate()
					};

					if (!!$scope.data && !!$scope.data.Day) {
						day = $scope.data.Day;
						if (!!$scope.data.Breakfast) {
							breakfast = $scope.data.Breakfast;
						}
						if (!!$scope.data.Lunch) {
							lunch = $scope.data.Lunch;
						}
						if (!!$scope.data.Dinner) {
							dinner = $scope.data.Dinner;
						}
					}

					$scope.Editable = {
						Day: day,
						Breakfast: breakfast,
						Lunch: lunch,
						Dinner: dinner,
					};

					$scope.CurrentMeal = 'Breakfast';
				};
				$scope.ResetEditable();

				$scope.GetDayString = function() {
					var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

					if (!$scope.Editable || !$scope.Editable.Day) return "No day selected";
					var day = $scope.Editable.Day;

					return monthNames[day.Month] + " " + day.DayOfMonth + ", " + day.FullYear;
				};

				$scope.$watch('data', function() {
					$scope.ResetEditable();
				});

				$scope.$on('MealChosen', function(event, args) {
					if ($scope.CurrentMeal == 'Breakfast') {
						$scope.CurrentMeal = 'Lunch';
					} else if ($scope.CurrentMeal == 'Lunch') {
						$scope.CurrentMeal = 'Dinner';
					} else if ($scope.CurrentMeal == 'Dinner') {
						$scope.Save($scope.Editable);

						var currentDate = $scope.data.Day;
						var currentDateJs = new Date(currentDate.FullYear, currentDate.Month, currentDate.DayOfMonth);
						var nextDateJs = new Date(currentDateJs.getTime() + (24 * 60 * 60 * 1000));
						$scope.$emit('SelectDay', { SelectedDate: nextDateJs });
					}
				});
			}]
		};
	});
})();
