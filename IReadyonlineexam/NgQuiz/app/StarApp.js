var starApp = angular.module('starApp', []);

starApp.controller('StarCtrl', ['$scope','$http', function ($scope,$http) {
    $scope.rating = 0;
    $scope.ratings = [ {
        current: 3,
        max: 5
    }];

    $scope.getSelectedRating = function (rating) {
        console.log(rating);
    }

    $scope.setMinrate = function () {
        $scope.ratings = [ {
            current: 1,
            max: 5
        }];
    }

    $scope.setMaxrate = function () {
        $scope.ratings = [ {
            current: 5,
            max: 5
        }];
    }

    $scope.sendRate = function () {
        alert("Thanks for your valuable rating" + $scope.ratings[0].current + "/" + $scope.ratings[0].max)
        $http.post('/someUrl', data, config).then(successCallback, errorCallback);
    }
}]);

starApp.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&'
        },
        link: function (scope, elem, attrs) {

            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                scope.ratingValue = index + 1;
                scope.onRatingSelected({
                    rating: index + 1
                });
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                if (newVal) {
                    updateStars();
                }
            });
        }
    }
});