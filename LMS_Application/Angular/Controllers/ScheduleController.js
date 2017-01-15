(function () {

    var ScheduleController = function ($scope, Request, Schedule, $routeParams) {
        var canvas = document.getElementById("ScheduleCanvas");
        var spinner = document.getElementById("canvasLoading");

        canvas.style.opacity = 0;

        Request.Make("/Account/GetAntiForgeryToken/", "get").then(function (token) {
            Request.Make("/Schedule/GetSchedule/", "get", null, null, { 'RequestVerificationToken': token.data }).then(function (res) {
                Schedule.Initialize(canvas, 5, false, "08:00", "17:00", res.data);

                spinner.style.opacity = 0;
                canvas.style.opacity = 1;
            });
        });

        // Set input and select fields to disabled //
        var setDisabled = function () {
            angular.forEach(document.getElementsByName("courseForm")[0].querySelectorAll("input, select, button[type=submit]"), function (value, key) {
                value.setAttribute("disabled", true);
            });
        }

        // Create new school class //
        var sendForm = function (isValid) {
            if (isValid) {
                if ($scope.currentPage == "remove") {
                    Popup.Message("Are you sure?", "You cant revert this back!", Popup.types.warning, { confirmText: "Remove", enableCancel: true }).then(function (res) {
                        if (res === true) {
                            PostForm();
                        }
                    });
                }
                else {
                    PostForm();
                }
            }
            else {
                Popup.Message("Sorry", "You need to enter all information before you can create a new class.", Popup.types.error, { confirmText: "Okey" });
            }
        }

        // Post form //
        function PostForm() {
            var schoolClassTmp = angular.copy($scope.class);
            schoolClassTmp.validTo = $filter('date')(schoolClassTmp.validTo, "yyyy-MM-dd");

            Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                Request.Make(sendFormTo, "post", JSON.stringify(schoolClassTmp), null, { 'RequestVerificationToken': token.data }).then(function (res) {
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
                        Popup.Message("Success!", popupResponseMessage, Popup.types.ok, { timer: 5000 }).then(function (res) {
                            $route.reload();
                        });
                    }
                });
            });
        }


        // Variables //
        $scope.course = {};

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/Schedule/_create.html",
            edit: basePath + "/Schedule/_edit.html",
            remove: basePath + "/Schedule/_remove.html",
            form: basePath + "/Schedule/_form.html"
        };

        // Set form post destination //
        var sendFormTo;
        var popupResponseMessage;
        var toDisable;
        switch ($scope.currentPage) {
            case "create":
                sendFormTo = "/Schedule/Create/";
                popupResponseMessage = "New course has been created";
                $scope.submitBtnText = "Create";
                break;
            case "edit":
                sendFormTo = "/Schedule/Update/";
                popupResponseMessage = "Course has been updated";
                toDisable = "input, select, button[type=submit]";
                $scope.submitBtnText = "Update";
                break;
            case "remove":
                sendFormTo = "/Schedule/Remove/";
                popupResponseMessage = "Course has been removed";
                toDisable = "button[type=submit]";
                $scope.submitBtnText = "Remove";
                break;
        }
    }

    LMSApp.controller('ScheduleController', [
        '$scope',
        'Request',
        'Schedule',
        '$routeParams',
        ScheduleController
    ]);

}());