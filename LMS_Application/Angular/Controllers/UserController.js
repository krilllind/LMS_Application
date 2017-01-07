(function () {

    var UserController = function ($scope, Request, Popup) {

        var sendForm = function (isValid) {
            if (isValid) {
                Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                    Request.Make("/Account/Register/", "post", JSON.stringify($scope.user), null, { 'RequestVerificationToken': token.data }).then(function (res) {
                        if (res.status.error) {
                            var fields = JSON.parse(res.message);
                            delete fields["$id"];

                            angular.forEach(fields, function (value, key) {
                                if (value.length != 0) {
                                    $scope.registerForm[key].valueUsedMessage = value.join("<br>");
                                    $scope.registerForm[key].$setValidity("valueUsed", false);
                                }
                            });
                        }
                        else {
                            Popup.Message("Added!", "New user " + $scope.user.firstname + " has been added.", Popup.types.ok, {
                                timer: 5000
                            }).then(function (response) {
                                angular.copy($scope.user, $scope.copyUser);
                                $scope.user = {}
                                $scope.user.userRole = $scope.copyUser.userRole;
                            });
                        }
                    });
                });
            }
            else {
                Popup.Message("Sorry", "You need to enter all information before you can add a new user.", Popup.types.error, {
                    confirmText: "Okey"
                }).then(function (response) {
                    if (response != false) {
                        $scope.user.password = "";
                        $scope.user.confirmpassword = "";
                    }
                });
            }
        }

        Request.Make("/Data/GetAllRoleNames/", "get").then(function (res) {
            $scope.user.userRole = res.data[0];
            $scope.roles = res.data;
        });

        $scope.user = {};
        $scope.SendForm = sendForm;
    }

    LMSApp.controller("UserController", [
        "$scope",
        "Request",
        "Popup",
        UserController
    ]);

}());