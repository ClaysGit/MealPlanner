(function() {
	angular.module("MealPlanner")
	.directive("mealOptions", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealOptions.html',
			scope: {
				data: "="
			},
			controller: ['$scope', function($scope) {
				$scope.Remove = function(option) {
					$scope.$emit('RemoveMealOption', option);
				};

				$scope.Add = function(editable) {
					$scope.$emit('AddMealOption', editable);
					$scope.ResetEditable();
				};

				$scope.OnNewIngredientKeyDown = function(keyCode) {
					if (keyCode === 13) {
						$scope.Editable.KeyIngredients.push($scope.Editable.NewIngredient);
						$scope.Editable.NewIngredient = '';
					}
				}

				$scope.ResetEditable = function() {
					$scope.Editable = {
						Name: '',
						NewIngredient: '',
						KeyIngredients: []
					};
				};
				$scope.ResetEditable();
			}]
		};
	});
})();
