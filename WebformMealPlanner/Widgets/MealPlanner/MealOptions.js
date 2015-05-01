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
					angular.forEach($scope.data.MealOptions, function(option) {
						if (editable.Name == option.Name) {
							alert('Pick a unique recipe name');
							return;
						}
					});

					$scope.$emit('AddMealOption', editable);
					$scope.ResetEditable();
				};

				$scope.Edit = function(option) {
					$scope.Editable = {
						Name: option.Name,
						NewIngredient: '',
						KeyIngredients: option.KeyIngredients
					};

					$scope.Remove(option);
				};

				$scope.OnNewIngredientKeyDown = function(keyCode) {
					if (keyCode === 13) {
						$scope.Editable.KeyIngredients.push($scope.Editable.NewIngredient);
						$scope.Editable.NewIngredient = '';
					}
				};

				$scope.RemoveIngredient = function(ingredient) {
					for (var i = 0; i < $scope.Editable.KeyIngredients.length; ++i) {
						if ($scope.Editable.KeyIngredients[i] == ingredient) {
							$scope.Editable.KeyIngredients.splice(i, 1);
							return;
						}
					}
				};

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
