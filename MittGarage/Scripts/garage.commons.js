angular.module('Garage.Commons', [])
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

                 GarageService.getVehicles = function ($scope) {
                     // Retrieve all vehicles into $scope.allVehicles and
                     // $scope.
                     return $http.get('/Items/GetVehicles')
                         .success(function (_vehicles) {
                             $scope.allVehicles = _vehicles ? _vehicles : "Nothing";
                             $scope.vehicles =
                                 JSON.parse(JSON.stringify($scope.allVehicles));
                             console.debug($scope.vehicles);
                         })
                        .error(function (error) {
                            $scope.status =
                                'Error loading vehicle data: ' + error.message;
                            console.error($scope.status);
                        });
                 };
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



angular.module('Garage.Admin',
               ['angularUtils.directives.dirPagination', 'Garage.Commons'])
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
        function checkDate(datestring) {
            // Return true if given string as as of 'today'
            var date = new Date(parseInt(datestring.substr(6)))
            var now = new Date()
            return date.getDay() == now.getDay() &&
                   date.getMonth() == now.getMonth() &&
                   date.getYear() == now.getYear();
        }
        $scope.getVehicles = function () {
            GarageService.getVehicles($scope)
        }
        $scope.onFilterButton = function () {
            // Update $scope.vehicles to match current filter conditions.
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
        GarageService.getTypes($scope);
        $scope.showDetails = false
        $scope.searchString = ""
        $scope.selectedType = "-"
        $scope.onlyToday = false
        $scope.getVehicles()
    })

