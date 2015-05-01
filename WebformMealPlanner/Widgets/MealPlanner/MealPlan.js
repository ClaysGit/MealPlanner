(function() {
	angular.module("MealPlanner")
	.directive("mealPlan", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealPlan.html',
			scope: {
				data: "=",
				mealOptions: "=",
				calendarDays: "=",
				selectedDay: "="
			},
			controller: ['$scope', function($scope) {
				
			}]
		};
	});
})();
