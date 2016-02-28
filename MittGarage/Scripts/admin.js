var GarageApp = angular.module('GarageApp', []);


GarageApp.factory('GarageService', ['$http', function ($http) {
    var GarageService = {};

    GarageService.getVehicles = function () {
        return $http.get('/Items/GetVehicles');
    };
    GarageService.getTypes = function () {
        return $http.get('/Items/GetTypes');
    };
    return GarageService;
}]);

GarageApp.controller('GarageController', function ($scope, GarageService) {
    console.log("Starting controller.")
    $scope.getVehicles = function() {
        GarageService.getVehicles()
            .success(function (_vehicles) {
                $scope.vehicles = _vehicles ? _vehicles : "Nothing";
                console.log($scope.vehicles);
            })
            .error(function (error) {
                $scope.status = 'Unable to load garage vehicle data: ' +
                    error.message;
                console.log($scope.status);
            });

    }
    $scope.showDetails = false
    $scope.getVehicles()
    $scope.sëlectedType = "´car"
    GarageService.getTypes()
            .success(function (_types) {
                $scope.types = _types;
                console.log($scope.types);
            })
            .error(function (error) {
                $scope.status = 'Unable to load garage type data: ' +
                    error.message;
                console.log($scope.status);
            });
});

