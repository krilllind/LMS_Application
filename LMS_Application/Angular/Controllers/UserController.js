(function () {

    var UserController = function ($scope, Request, Popup, $routeParams, $route) {

        // Checks form validation before posting //
        var sendForm = function (isValid) {
            if (isValid) {
                if ($scope.currentPage == "remove") {
                    Popup.Message("Are you sure?", "You cant revert this back!", Popup.types.warning, { confirmText: "Remove", enableCancel: true }).then(function (res) {
                        if (res === true) {
                            postForm();
                        }
                    });
                }
                else {
                    postForm();
                }
            }
            else {
                Popup.Message("Sorry", "You need to enter all information before you can submit.", Popup.types.error, {
                    confirmText: "Okey"
                }).then(function (response) {
                    if (response != false) {
                        $scope.user.password = "";
                        $scope.user.confirmpassword = "";
                    }
                });
            }
        }

        // Post form //
        function postForm() {
            Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                Request.Make(sendFormTo, "post", JSON.stringify($scope.user), null, { 'RequestVerificationToken': token.data }).then(function (res) {
                    if (res.status.error) {
                        if (res.data == null) {
                            Popup.Message("Error", res.message, Popup.types.error, { confirmText: "Okey" });
                        }
                        else {
                            var fields = JSON.parse(res.data);
                            delete fields["$id"];

                            angular.forEach(fields, function (value, key) {
                                if (value.length != 0) {
                                    $scope.registerForm[key].valueUsedMessage = value.join("<br>");
                                    $scope.registerForm[key].$setValidity("valueUsed", false);
                                }
                            });
                        }
                    }
                    else {
                        Popup.Message("Success!", popupResponseMessage, Popup.types.ok, { timer: 5000 }).then(function (res) {
                            $route.reload();
                        });
                    }
                });
            });
        }

        // Set selected user to edit //
        var setSelectUser = function (ssn) {
            angular.forEach($scope.users, function (value, key) {
                if (value.ssn == ssn) {
                    $scope.user = angular.copy(value);
                    document.getElementsByName("registerForm")[0].querySelectorAll("button[type=submit]")[0].removeAttribute("disabled");
                    return;
                }
            });
        }

        // Set input and select fields to disabled //
        var setDisabled = function () {
            angular.forEach(document.getElementsByName("registerForm")[0].querySelectorAll("input, select, button[type=submit]"), function (value, key) {
                value.setAttribute("disabled", true);
            });
        }

        // Get all user roles //
        Request.Make("/Data/GetAllRoleNames/", "get").then(function (res) {
            $scope.roles = res.data;
        });

        // Get all users //
        Request.Make("/Data/GetAllUsers/", "get").then(function (res) {
            $scope.users = res.data;
        });

        // Get this user info //
        Request.Make("/Data/GetUserInformation/", "get").then(function (res) {
            $scope.currentUser = res.data;
        });

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/User/_create.html",
            edit: basePath + "/User/_edit.html",
            remove: basePath + "/User/_remove.html",
            form: basePath + "/User/_form.html",
            form_password: basePath + "/User/_form_password.html",
        };

        // Variables //
        $scope.user = {};
        $scope.SendForm = sendForm;
        $scope.SetSelectUser = setSelectUser;
        $scope.SetDisabled = setDisabled;

        // Set form post destination //
        var sendFormTo;
        var popupResponseMessage;
        switch ($scope.currentPage) {
            case "create":
                sendFormTo = "/Account/Register/";
                popupResponseMessage = "New user has been registerd!";
                $scope.submitBtnText = "Register";
                break;
            case "edit":
                sendFormTo = "/Data/UpdateUser/";
                popupResponseMessage = "User has successfully been updated!";
                $scope.submitBtnText = "Update";
                break;
            case "remove":
                sendFormTo = "/Data/RemoveUser/";
                popupResponseMessage = "User has successfully been removed!";
                $scope.submitBtnText = "Remove";
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