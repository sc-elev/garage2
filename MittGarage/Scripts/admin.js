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
    function filter(collection, predicate)
    {
        var result = new Array();
        var length = collection.length;

        for(var j = 0; j < length; j++) {
            if(predicate(collection[j]) == true) {
                result.push(collection[j]);
            }
        }
        return result;
    }

    $scope.getVehicles = function() {
        GarageService.getVehicles()
            .success(function (_vehicles) {
                $scope.allVehicles = _vehicles ? _vehicles : "Nothing";
                $scope.vehicles =
                    JSON.parse(JSON.stringify($scope.allVehicles));
                console.log($scope.vehicles);
            })
            .error(function (error) {
                $scope.status = 'Unable to load garage vehicle data: ' +
                    error.message;
                console.log($scope.status);
            });
    }
    $scope.onFilterButton = function () {
        $scope.vehicles = JSON.parse(JSON.stringify($scope.allVehicles));
        $scope.vehicles = filter($scope.vehicles, function(el) {
            return !$scope.searchString ||
                (el.RegNr && el.RegNr.indexOf($scope.searchString) != -1)
        })
        $scope.vehicles = filter($scope.vehicles, function (el) {
            return $scope.selectedType == '-' ||
                   $scope.selectedType == el.Typename;
        })
    }
    $scope.showDetails = false
    $scope.searchString = ""
    $scope.getVehicles()
    $scope.selectedType = "-"
    GarageService.getTypes()
            .success(function (_types) {
                $scope.types = _types;
                $scope.types.unshift("-")
                console.log($scope.types);
            })
            .error(function (error) {
                $scope.status = 'Unable to load garage type data: ' +
                    error.message;
                console.log($scope.status);
            });
});

