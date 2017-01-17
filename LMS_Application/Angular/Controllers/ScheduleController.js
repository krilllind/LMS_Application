(function () {
    var ScheduleController = function ($scope, Request, Schedule, Popup, $filter, $routeParams, $route, $timeout) {
        var canvas = document.getElementById("ScheduleCanvas");
        var spinner = document.getElementById("canvasLoading");

        canvas.style.opacity = 0;

        // Retrieve schedule //
        function renderSchedule(destination) {
            Request.Make("/Account/GetAntiForgeryToken/", "get").then(function (token) {
                Request.Make("/Schedule/" + destination + "/", "get", { schoolClassID: $scope.course.schoolClassID }, null, { 'RequestVerificationToken': token.data }).then(function (res) {
                    for (var i = 0; i < res.data.length; i++) {
                        res.data[i].from = res.data[i].from.substr(11, 5);
                        res.data[i].to = res.data[i].to.substr(11, 5);
                    }

                    Schedule.Initialize(canvas, 5, false, "08:30", "16:00", res.data);
                    spinner.style.opacity = 0;
                    canvas.style.opacity = 1;

                });
            });
        }

        // Set input and select fields to disabled //
        function SetDisabled() {
            angular.forEach(document.getElementsByName("courseForm")[0].querySelectorAll("input, select, button[type=submit]"), function (value, key) {
                value.setAttribute("disabled", true);
            });
        }

        // Create new school class //
        var sendForm = function (isValid) {
            console.log("sendForm");
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
            var courseTmp = angular.copy($scope.course);

            Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                Request.Make(sendFormTo, "post", JSON.stringify(courseTmp), null, { 'RequestVerificationToken': token.data }).then(function (res) {
                    if (res.status.error) {
                        Popup.Message("Sorry!", res.message, Popup.types.error);
                    }
                    else {
                        console.log(Popup);
                        Popup.Message("Success!", popupResponseMessage, Popup.types.ok, { timer: 5000 }).then(function (info) {
                            Schedule.AddLesson(courseTmp);
                        });
                    }
                });
            });
        }

        // Gets all classes from the server //
        Request.Make("/Data/GetAllClasses/", "get").then(function (res) {
            $scope.classes = res.data;
        });

        // Set selected school class from list //
        var setSelectedClass = function (id) {
            $scope.course.schoolClassID = id;

            $timeout(function () {
                renderSchedule("GetSchedule");
            }, 0);
        }

        // Variables //
        $scope.days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
        $scope.course = {
            day: $scope.days[0]
        };
        $scope.class = {};
        $scope.SendForm = sendForm;
        $scope.SetSelectedClass = setSelectedClass;

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/Schedule/_create.html",
            edit: basePath + "/Schedule/_edit.html",
            remove: basePath + "/Schedule/_remove.html",
            form: basePath + "/Schedule/_form.html"
        };

        // Render schedule //
        renderSchedule("GetMySchedule");

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
        'Popup',
        '$filter',
        '$route',
        '$timeout',
        ScheduleController
    ]);

}());