var GarageApp = angular.module('GarageApp', []);


GarageApp.factory('GarageService', ['$http', function ($http) {
    var GarageService = {};

    GarageService.getVehicles = function () {
        return $http.get('/Items/GetVehicles');
    };
    return GarageService;
}]);

GarageApp.controller('GarageController', function ($scope, GarageService) {

    $scope.getVehicles = function() {
        GarageService.getVehicles()
            .success(function (_vehicles) {
                $scope.vehicles = _vehicles ? _vehicles : "Nothing";
                console.log($scope.vehicles);
            })
            .error(function (error) {
                $scope.status = 'Unable to load garage data: ' + error.message;
                console.log($scope.status);
            });

    }
    $scope.getVehicles()

});

