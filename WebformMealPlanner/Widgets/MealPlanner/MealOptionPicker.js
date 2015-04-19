(function() {
	angular.module("MealPlanner")
	.directive("mealOptionPicker", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealOptionPicker.html',
			scope: {
				data: "=",
				choice: "="
			},
			controller: ['$scope', function($scope) {
				$scope.$watchCollection('data.MealOptions', function() {
					if ($scope.data.MealOptions.length > 0) {
						$scope.choice = $scope.data.MealOptions[0];
					}
				});
			}]
		};
	});
})();
