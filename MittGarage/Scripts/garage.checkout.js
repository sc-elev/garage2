angular.module('Garage.Checkout',
               ['Garage.Commons', 'ngRoute'], function ($locationProvider) {
                   $locationProvider.html5Mode({
                       enabled: true,
                       requireBase: false
                   })
               })
.controller('CheckoutController', ['$scope', '$location', 'GarageService',
    function ($scope, $location, GarageService) {
        console.debug('Starting controller')
        function filter(collection, predicate) {
            // Return collection of items where
            // predicate(item) is true.
            var result = new Array();
            for (var j = 0; j < collection.length; j++) {
                if (predicate(collection[j]))
                    result.push(collection[j]);
            }
            return result;
        }
        var id = $location.path().split('/').pop()
        GarageService.getVehicle($scope, id)
    }
])