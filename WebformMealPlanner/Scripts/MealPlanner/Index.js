(function() {
	angular.module("MealPlanner", ["ng"])
		.controller("MealPlannerController", ["$scope", "$http", function($scope, $http) {
			$scope.mealPlan = {};
			$scope.mealOptions = { MealOptions: [] };
			$scope.mode = 'MealPlan'; //MealPlan, MealOptions, Config

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

			$scope.GetMealPlan = function() {
				$http.post('MealPlanner/GetMealPlan')
				.success(function(response) {
					$scope.mealPlan = response;
				})
				.error(function(response) {
					alert('There was a problem getting the meal plan from the server');
				});
			}

			$scope.GetMealOptions = function() {
				$http.post('MealPlanner/GetMealOptions')
				.success(function(response) {
					$scope.mealOptions = response;
				})
				.error(function(response) {
					alert('There was a problem getting the meal options from the server');
				});
			}

			$scope.GetMealPlan();
			$scope.GetMealOptions();
		}]);
})();
