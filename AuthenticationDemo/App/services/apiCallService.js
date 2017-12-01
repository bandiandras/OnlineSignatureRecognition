(function () {
    'use strict';

    angular
        .module('app')
        .service('ApiCall', ApiCall);

    ApiCall.$inject = ['$rootScope', '$http'];
    function ApiCall($rootScope, $http) {
        var result;

        // this is used for calling get methods from web api
        this.GetApiCall = function (controllername, methodname) {
            result = $http.get('api/' + controllername + '/' + methodname).success(function (data) {
                result = data;
            }).error(function () {
                alert("something went wrong");
            });
            return result;
        }

        // this is used for calling get methods from web api
        this.GetApiCallOneParam = function (controllername, methodname, paramName, paramValue) {
            $http.get('api/' + controllername + '/' + methodname + '?' + paramName + '=' + paramValue).success(function (data) {
                result = data.data;
            }).error(function () {
                alert("something went wrong");
            });
            return result;
        }

        // this is used for calling post methods from web api with passing some data to the web api controller
        this.PostApiCall = function (controllername, methodname, obj) {
            result = $http.post('api/' + controllername + '/' + methodname, obj).success(function (data, status) {
                result = (data);
            }).error(function () {
                alert("something went wrong");
            });
            return result;
        }
    }
  

})();