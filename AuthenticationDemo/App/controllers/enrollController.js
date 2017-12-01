(function () {
    'use strict';

    angular
        .module('app')
        .controller('EnrollController', EnrollController);

    EnrollController.$inject = ['$scope', '$rootScope', '$location'];
    function EnrollController($scope, $rootScope, $location) {
        var vm = this;

        $scope.enroll = function () {
            $rootScope.email = $scope.email;
            let request = $.ajax({
                method: "GET",
                url: $rootScope.url + 'User/CheckIfUserExists?Email=' + $scope.email,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false
            });

            request.done(function (data) {
                $scope.result = data;
            });

            if ($scope.result === 0) {
                $location.path('register');
            }
            else {
                toastr.error('User already exists!');
            }
        };
    }

})();