angular.module('GarageApp',
               ['angularUtils.directives.dirPagination'])
    .filter("jsDate", function () {
            return function (x) {
                return new Date(parseInt(x.substr(6)))
            }
    })
    .factory('GarageService',
             ['$http', function ($http) {
                 // Retrieve lists of all vehicles and vehicle types
                 // from server.
                 var GarageService = {};

                 GarageService.getVehicles = function () {
                     return $http.get('/Items/GetVehicles');
                 };
                 GarageService.getTypes = function () {
                     return $http.get('/Items/GetTypes');
                 };
                 return GarageService;
             }]
    )
    .controller('GarageController', function ($scope, GarageService) {
        console.debug('Starting controller')
        function filter(collection, predicate) {
            // Return collection of items where  predicate(item) is true.
            var result = new Array();
            for (var j = 0; j < collection.length; j++) {
                if (predicate(collection[j]))
                    result.push(collection[j]);
            }
            return result;
        }
        $scope.getVehicles = function () {
            // Retrieve all vehicles into $scope.allVehicles and
            // $scope.véhicles
            GarageService.getVehicles()
                .success(function (_vehicles) {
                    $scope.allVehicles = _vehicles ? _vehicles : "Nothing";
                    $scope.vehicles =
                        JSON.parse(JSON.stringify($scope.allVehicles));
                    console.debug($scope.vehicles);
                })
                .error(function (error) {
                    $scope.status =
                        'Error loading garage vehicle data: ' + error.message;
                    console.error($scope.status);
                });
        }
        var checkDate = function (datestring) {
            // Return true if given string as as of 'today'
            var date = new Date(parseInt(datestring.substr(6)))
            var now = new Date()
            return date.getDay() == now.getDay() &&
                   date.getMonth() == now.getMonth() &&
                   date.getYear() == now.getYear();
        }
        $scope.onFilterButton = function () {
            // Update $scope.vehicles to match current filter setup.
            $scope.vehicles = JSON.parse(JSON.stringify($scope.allVehicles));
            $scope.vehicles = filter($scope.vehicles, function (el) {
                return !$scope.searchString ||
                    (el.RegNr && el.RegNr.indexOf($scope.searchString) != -1) ||
                    (el.OwnerName &&
                         el.OwnerName.indexOf($scope.searchString) != -1)
            })
            $scope.vehicles = filter($scope.vehicles, function (el) {
                return $scope.selectedType == '-' ||
                       $scope.selectedType == el.Typename;
            })
            $scope.vehicles = filter($scope.vehicles, function (el) {
                return !$scope.onlyToday || checkDate(el.checkInDate)
            })
        }
        $scope.onDetails = function () {
            // Show/hide details section and update button label.
            $scope.showDetails = !$scope.showDetails;
            var label = document.getElementById('detailsButton').value
            if ($scope.showDetails)
                label = label.replace('>>>', '<<<')
            else
                label = label.replace('<<<', '>>>')
            document.getElementById('detailsButton').value = label
        }
        GarageService.getTypes()
                // Retrieve available vehicle types into $scope.types.
                .success(function (_types) {
                    $scope.types = _types;
                    $scope.types.unshift("-")
                    console.debug($scope.types);
                })
                .error(function (error) {
                    $scope.status = 'Unable to load garage type data: ' +
                        error.message;
                    console.error($scope.status);
                });
        $scope.showDetails = false
        $scope.searchString = ""
        $scope.selectedType = "-"
        $scope.onlyToday = false
        $scope.getVehicles()
    }
);
