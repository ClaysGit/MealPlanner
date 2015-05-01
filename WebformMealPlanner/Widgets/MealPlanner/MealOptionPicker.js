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
				$scope.GetButtonType = function(option) {
					if (!!$scope.choice && option.Name == $scope.choice.Name) {
						return "btn-success";
					}

					return "btn-primary";
				};

				$scope.SelectOption = function(option) {
					$scope.choice = option;
					$scope.$emit('MealChosen');
				};

				$scope.$watchCollection('data.MealOptions', function() {
					if (!!$scope.data && !!$scope.data.MealOptions && $scope.data.MealOptions.length > 0) {
						$scope.choice = $scope.data.MealOptions[0];
					}
				});

				$scope.$watch('choice', function(newC, oldC) {
					if (!$scope.choice) {
						if (!!$scope.data && !!$scope.data.MealOptions && $scope.data.MealOptions.length > 0) {
							$scope.choice = $scope.data.MealOptions[0];
						}
					}
					else if (!!$scope.choice && !$scope.choice.$$hashKey) {
						for (var i = 0; i < $scope.data.MealOptions.length; ++i) {
							if ($scope.data.MealOptions[i].Name == $scope.choice.Name) {
								$scope.choice = $scope.data.MealOptions[i];
								return;
							}
						}
					}
				});
			}]
		};
	});
})();
