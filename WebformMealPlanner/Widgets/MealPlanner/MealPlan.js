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
				$scope.activeDay = null;
			}]
		};
	});
})();
