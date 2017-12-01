(function () {
    'use strict';

    angular.module('app', [
        // Angular modules
        'ngCookies',
        'ngAnimate',
        'ngRoute'

        // Custom modules

        // 3rd Party Modules        
    ])

        //configure routing
    .config(function($routeProvider) {
        $routeProvider
        .when("/", {
            templateUrl: "home.html"
        })
        .when("/enroll", {
            templateUrl: "enroll.html"
        })
        .when("/sign", {
            templateUrl: "registerSignature.html"
        })
        .when("/register", {
            templateUrl: "register.html"
        });
    });
})();
