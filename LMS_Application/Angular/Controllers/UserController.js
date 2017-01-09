(function () {

    var UserController = function ($scope, Request, Popup, $routeParams, $route) {

        // Send user form //
        var sendForm = function (isValid) {
            if (isValid) {
                Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                    Request.Make(sendFormTo, "post", JSON.stringify($scope.user), null, { 'RequestVerificationToken': token.data }).then(function (res) {
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
                            Popup.Message("Success!", "Database has been updated with user " + $scope.user.firstname, Popup.types.ok, { timer: 5000 }).then(function (res) {
                                $route.reload();
                            });
                        }
                    });
                });
            }
            else {
                Popup.Message("Sorry", "You need to enter all information before you can submit a user.", Popup.types.error, {
                    confirmText: "Okey"
                }).then(function (response) {
                    if (response != false) {
                        $scope.user.password = "";
                        $scope.user.confirmpassword = "";
                    }
                });
            }
        }

        // Set selected user to edit //
        var setSelectUser = function (ssn) {
            angular.forEach($scope.users, function (value, key) {
                if (value.ssn == ssn) {
                    $scope.user = angular.copy(value);
                    return;
                }
            });
        }

        // Get all user roles //
        Request.Make("/Data/GetAllRoleNames/", "get").then(function (res) {
            $scope.roles = res.data;
        });

        // Get all users //
        Request.Make("/Data/GetAllUsers/", "get").then(function (res) {
            $scope.users = res.data;
            console.log(res.data);
        });

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/User/_create.html",
            edit: basePath + "/User/_edit.html",
            remove: basePath + "/User/_remove.html",
            form: basePath + "/User/_form.html"
        };

        // Variables //
        $scope.user = {};
        $scope.SendForm = sendForm;
        $scope.SetSelectUser = setSelectUser;

        // Set form post destination //
        var sendFormTo;
        switch ($scope.currentPage) {
            case "create":
                sendFormTo = "/Account/Register/";
                break;
            case "edit":
                sendFormTo = "/Data/UpdateUser/";
                break;
        }
    }

    LMSApp.controller("UserController", [
        "$scope",
        "Request",
        "Popup",
        "$routeParams",
        "$route",
        UserController
    ]);

}());