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
				$scope.Delete = function(data) {
					$scope.$emit('RemoveMealPlanDay', data);
					$scope.ResetEditable();
				};

				$scope.Save = function(data) {
					$scope.$emit('AddMealPlanDay', data);
					$scope.ResetEditable();
				};

				$scope.ResetEditable = function() {
					var choice = {};
					if ($scope.mealOptions && $scope.mealOptions.MealOptions.length > 0) {
						choice = $scope.mealOptions.MealOptions[0];
					}

					$scope.Editable = {
						Breakfast: choice,
						Lunch: choice,
						Dinner: choice,
					};
				};

				$scope.GetDayString = function() {
					var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

					if (!$scope.data || !$scope.data.Day) return "No day selected";
					var day = $scope.data.Day;

					return monthNames[day.Month] + " " + day.DayOfMonth + ", " + day.FullYear;
				};

				$scope.$watch('data.Breakfast', function() {
					if ($scope.data.Breakfast == null && $scope.mealOptions && $scope.mealOptions.MealOptions.length > 0) {
						$scope.data.Breakfast = $scope.mealOptions.MealOptions[0];
					}
				});
				$scope.$watch('data.Lunch', function() {
					if ($scope.data.Lunch == null && $scope.mealOptions && $scope.mealOptions.MealOptions.length > 0) {
						$scope.data.Lunch = $scope.mealOptions.MealOptions[0];
					}
				});
				$scope.$watch('data.Dinner', function() {
					if ($scope.data.Dinner == null && $scope.mealOptions && $scope.mealOptions.MealOptions.length > 0) {
						$scope.data.Dinner = $scope.mealOptions.MealOptions[0];
					}
				});
			}]
		};
	});
})();
