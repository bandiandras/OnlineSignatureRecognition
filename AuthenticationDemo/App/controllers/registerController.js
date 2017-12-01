(function () {
    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['$scope', '$rootScope', '$location', 'CryptoService'];
    function RegisterController($scope, $rootScope, $location, CryptoService) {
        var vm = this;

        let sampleCounter = 1;
        $scope.sampleCounter = sampleCounter;

        var wrapper = document.getElementById("signature-pad"),
           canvas = wrapper.querySelector("canvas"),
           name = document.getElementById("nameField"),
           signaturePad;

        // Adjust canvas coordinate space taking into account pixel ratio,
        // to make it look crisp on mobile devices.
        // This also causes canvas to be cleared.
        function resizeCanvas() {
            // When zoomed out to less than 100%, for some very strange reason,
            // some browsers report devicePixelRatio as less than 1
            // and only part of the canvas is cleared then.
            var ratio = Math.max(window.devicePixelRatio || 1, 1);
            canvas.width = canvas.offsetWidth * ratio;
            canvas.height = canvas.offsetHeight * ratio;
            canvas.getContext("2d").scale(ratio, ratio);
        }

        window.onresize = resizeCanvas;
        resizeCanvas();

        signaturePad = new SignaturePad(canvas);
        
        $scope.clear = function () {
            signaturePad.clear();
        };

        $scope.save = function () {
            var points = []
            if (signaturePad.isEmpty()) {
                toastr.info('Please provide your signature first!');
            } else {
                if (sampleCounter <= 9) {
                    let sigToSend = new SigToSend(signaturePad.getPoints(), $rootScope.email);

                    //Encrypt the signature before sending
                    CryptoService.Encrypt(sigToSend);

                    var request = $.ajax({
                        method: "POST",
                        url: $rootScope.url + "Signature/SaveSignature",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'application/json',
                        data: JSON.stringify(sigToSend),
                        async: false
                    })

                    if (request.responseText == 1) {
                        sampleCounter++;
                        $scope.sampleCounter++;
                        signaturePad.clear();
                    }
                    else {
                        toastr.error('Signature quality error! Please provide the sample once again!');
                    }

                }
                else {
                    //use window.location to reload the whole page
                    window.location = '/Signature';
                }
            }
        };

    }

})();