(function () {

    var loginController = function ($scope, Request, Popup, $timeout) {

        // Functions
        var sendForm = function (isValid) {
            if (isValid) {
                Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                    Request.Make("/Account/Login/", "post", JSON.stringify($scope.user), { 'RequestVerificationToken': token.data }).then(function (res) {
                        if (res.status.error) {
                            if (res.data == null) {
                                // No validation messages
                                Popup.Message("Error", res.message, Popup.types.error, {
                                    confirmText: "Okey"
                                }).then(function (response) {
                                    if (response != false) {
                                        $timeout(function () {
                                            $scope.user.password = "";
                                        }, 0);
                                    }
                                });
                            }
                            else {
                                // Show validation errors
                                var fields = JSON.parse(res.data);
                                delete fields["$id"];

                                angular.forEach(fields, function (value, key) {
                                    console.log(key);

                                    if (value.length != 0) {
                                        $scope.loginForm[key].valueUsedMessage = value.join("<br>");
                                        $scope.loginForm[key].$setValidity("valueUsed", false);
                                    }
                                });
                            }
                        }
                        else {
                            window.location.pathname = "/";
                        }
                    });
                });
            }
            else {
                Popup.Message("Sorry", "You need to enter both email and password before you can login", Popup.types.error, {
                    confirmText: "Okey"
                }).then(function (response) {
                    if (response != false) {
                        $scope.user.password = "";
                    }
                });
            }
        }

        // Variables
        $scope.user = {};
        $scope.SendForm = sendForm;
    }

    LMSApp.controller("LoginController", [
        "$scope",
        "Request",
        "Popup",
        "$timeout",
        loginController
    ]);

}());