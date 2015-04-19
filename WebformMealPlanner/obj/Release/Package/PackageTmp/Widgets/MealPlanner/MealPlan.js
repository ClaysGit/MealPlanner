(function() {
	angular.module("MealPlanner")
	.directive("mealPlan", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealPlan.html',
			scope: {
				data: "=",
				mealOptions: "="
			},
			controller: ['$scope', function($scope) {
				$scope.Remove = function(dayPlan) {
					$scope.$emit('RemoveMealPlanDay', dayPlan);
				};

				$scope.Add = function(editable) {
					$scope.$emit('AddMealPlanDay', editable);
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
				$scope.ResetEditable();
			}]
		};
	});
})();
