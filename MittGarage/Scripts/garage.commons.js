angular.module('Garage.Commons', ['ngRoute'])
    .filter("jsDate", function () {
        return function (x) {
            return new Date(parseInt(x.substr(6)))
        }
    })
    .factory('GarageService',
             ['$http', '$timeout', function ($http, $timeout) {
                 var GarageService = {};
                 function collectionFilter(collection, predicate) {
                     // Return collection of items where predicate(item) is true.
                     if (!collection)
                         return collection
                     var result = new Array();
                     for (var j = 0; j < collection.length; j++) {
                         if (predicate(collection[j]))
                             result.push(collection[j]);
                     }
                     return result;
                 }
                 GarageService._getVehicles = function($scope, id) {
                     // Retrieve all vehicles into $scope.allVehicles and
                     // $scope.vehicles
                     $scope.showLoadMessage = true
                     return $http.get('/Items/GetVehicles')
                         .success(function (_vehicles) {
                             $scope.allVehicles = _vehicles ? _vehicles : "Nothing";
                             $scope.vehicles =
                                 JSON.parse(JSON.stringify($scope.allVehicles));
                             console.debug($scope.vehicles)
                             $scope.startFade = true
                             $timeout(function () {
                                          $scope.showLoadMessage = false },
                                      2000)
                             if (id == -1)
                                 return
                             var vehicles = collectionFilter(
                                 $scope.vehicles, function (el) {
                                     return el.Id == id
                                 })
                             $scope.vehicle = vehicles[0];

                         })
                        .error(function (error) {
                            $scope.status =
                                'Error loading vehicle data: ' + error.message;
                            console.error($scope.status);
                        })
                 }
                 GarageService.getVehicles = function ($scope) {
                     // Retrieve all vehicles into $scope.allVehicles and
                     // $scope.vehicles
                     GarageService._getVehicles($scope, -1)
                 }
                 GarageService.getVehicle = function ($scope, id) {
                     // Retrieve a single, existing vehicle with given id.
                     GarageService._getVehicles($scope, id)
                 }
                 GarageService.getTypes = function ($scope) {
                     // Retrieve available vehicle types into $scope.types.
                     return $http.get('/Items/GetTypes')
                        .success(function (_types) {
                            $scope.types = _types;
                            $scope.types.unshift("-")
                            console.debug($scope.types);
                        })
                        .error(function (error) {
                            $scope.status = 'Unable to load garage type data: '
                                + error.message;
                            console.error($scope.status);
                        });
                 };
                 return GarageService;
             }]
    )
