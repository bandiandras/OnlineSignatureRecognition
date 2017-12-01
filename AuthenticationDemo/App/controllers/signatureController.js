(function () {
    angular
        .module('app')
        .controller('SignatureController', SignatureController);

    SignatureController.$inject = ['$scope', '$rootScope', '$cookies', 'CryptoService'];
    function SignatureController($scope, $rootScope, $cookies, CryptoService) {
        var vm = this;
        var wrapper = document.getElementById("signature-pad"),
           //clearButton = wrapper.querySelector("[data-action=clear]"),
           //saveButton = wrapper.querySelector("[data-action=save]"),
            canvas = wrapper.querySelector("canvas"),
            name = document.getElementById("nameField");
        var signaturePad;


        vm.init = function () {
            signaturePad = new SignaturePad(canvas);          
        };

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
        vm.init();
        window.onresize = resizeCanvas;
        resizeCanvas();

        $scope.clear = function()
        {
            signaturePad.clear();
        }

        $scope.save = function () {
            var points = []
            if (signaturePad.isEmpty()) {
                toastr.info('Please provide your signature first!');
            } else {
                let sigToSend = new SigToSend(signaturePad.getPoints(), $rootScope.email);

                //Encrypt the signature before sending
                CryptoService.Encrypt(sigToSend);

                let request = $.ajax({
                    method: "POST",
                    url: $rootScope.url + "Signature/CheckSignature",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'application/json',
                    data: JSON.stringify(sigToSend),
                    async: false
                })

                //if the signature is accepted, set a cookie, with the logged id username
                if (request.responseText == "true") {
                    //$cookies.put("activeUser", $rootScope.email);
                    toastr.success('Authentication succesfull!');

                }
                else {
                    toastr.error('Authentication failed!');
                }
            }
        };

        $scope.checkboxModel = {
            value: false
        };

        $scope.$watch('checkboxModel.value', function (newValue, oldValue) {
            signaturePad.off();
            delete signaturePad.canvas;
            if (newValue === true)
            {
                var options = new SignaturePadSettings("white");                
                signaturePad = new SignaturePad(canvas, options);
            }
            else {
                signaturePad = new SignaturePad(canvas);
            }
        });
    }

})();