(function () {
    'use strict';

    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$scope', '$location', '$rootScope'];
    function HomeController($scope, $location, $rootScope) {
        var vm = this;

        vm.init = function () {
            //URL
            $rootScope.url = "http://192.168.1.100/Signature/api/";
        }
        
        vm.init();

        $scope.enroll = function () {
            $location.path('enroll');
        };

        //Calls CheckIfUserExists method from the API of the server 
        $scope.login = function () {
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

            if ($scope.result) {
                $location.path('sign');
            }
            else {
                toastr.info('Username not recognized!');
            }
        };
    }

})();