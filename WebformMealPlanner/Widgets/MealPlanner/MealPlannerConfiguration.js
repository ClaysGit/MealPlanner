(function() {
	angular.module("MealPlanner")
	.directive("mealPlan", function() {
		return {
			restrict: 'E',
			templateUrl: 'Widgets/MealPlanner/MealPlan.html',
			scope: {
			    data: "=",
                defaultEngineData: "="
			},
			controller: ['$scope', function($scope) {
			    $scope.Editable = {};
			    $scope.ResetEditable = function () {
			        $scope.Editable = {};

			        $scope.Editable.EnginePort = $scope.data.EngineServicePort || "";
			        $scope.Editable.EngineHost = $scope.data.EngineServiceHostName || "";
			        $scope.Editable.WebPort = $scope.data.WebServicePort || "";
			        $scope.Editable.WebHost = $scope.data.WebServiceHostName || "";
			        $scope.Editable.IconPort = $scope.data.NotifyIconServicePort || "";
			        $scope.Editable.IconHost = $scope.data.NotifyIconServiceHostName || "";
			        $scope.Editable.DaysToPlan = $scope.data.DaysToPlan || "";
			        $scope.Editable.DaysToShop = $scope.data.ShoppingDaysNeeded || "";
			    };
			    $scope.ResetEditable();

			    $scope.DefaultEngineEditable = {};
			    $scope.ResetDefaultEngineEditable = function () {
			        $scope.DefaultEngineEditable = {};

			        $scope.DefaultEngineEditable.EnginePort = $scope.defaultEngineData.Port || "";
			        $scope.DefaultEngineEditable.EngineHost = $scope.defaultEngineData.HostName || "";
			    };
			    $scope.ResetDefaultEngineEditable();
			}]
		};
	});
})();
