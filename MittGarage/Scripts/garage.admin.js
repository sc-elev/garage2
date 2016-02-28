angular.module('Garage.Admin',
               ['angularUtils.directives.dirPagination', 'Garage.Commons', 'gettext'])
    .run(function (gettextCatalog) {
        gettextCatalog.setCurrentLanguage('sv_SE')
    })
    .controller('GarageController', function ($scope, $timeout, GarageService) {
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
        function isToday(datestring) {
            // Return true if given string as as of 'today'
            var date = new Date(parseInt(datestring.substr(6)))
            var now = new Date()
            return date.getDay() == now.getDay() &&
                   date.getMonth() == now.getMonth() &&
                   date.getYear() == now.getYear();
        }
        $scope.getVehicles = function () {
            GarageService.getVehicles($scope, $timeout)
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
                return !$scope.onlyToday || isToday(el.checkInDate)
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

        $scope.getVehicles()
    })