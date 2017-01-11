(function () {

    var ClassController = function ($scope, Request, Popup, $filter, $routeParams, $route) {

        // Add student to school class //
        var addToClass = function (student) {
            if ($scope.selectedClass) {
                var index = $scope.students.indexOf(student);
                if (index != -1)
                    $scope.students.splice(index, 1);

                Request.Make("/Data/AddStudentsToClass/", "post", JSON.stringify({ classID: $scope.selectedClass.schoolClassID, studentSSN: student.ssn })).then(function (res) {
                    $scope.selectedClass.students.push(student);
                });
            }
            else {
                Popup.Message("Sorry", "You need to select a school class before you can add students.", Popup.types.info, { confirmText: "Got it!" });
            }
        }

        // Remove student from school class //
        var removeFromClass = function (student) {
            var index = $scope.selectedClass.students.indexOf(student);
            if (index != -1)
                $scope.selectedClass.students.splice(index, 1);

            Request.Make("/Data/RemmoveStudentsFromClass/", "post", JSON.stringify({ classID: $scope.selectedClass.schoolClassID, studentSSN: student.ssn })).then(function (res) {
                $scope.students.push(student);
            });
        }

        // Set selected school class from list //
        var setSelectedClass = function (clsId) {
            angular.forEach($scope.classes, function (value, key) {
                if (value.schoolClassID == clsId) {
                    $scope.selectedClass = value;
                    $scope.class = angular.copy(value);
                    $scope.class.validTo = new Date($scope.class.validTo);

                    angular.forEach(document.getElementsByName("classForm")[0].querySelectorAll(toDisable), function (value, key) {
                        value.removeAttribute("disabled");
                    });

                    return;
                }
            });
        }

        // Set input and select fields to disabled //
        var setDisabled = function () {
            angular.forEach(document.getElementsByName("classForm")[0].querySelectorAll("input, select, button[type=submit]"), function (value, key) {
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

        // Gets all classes from the server //
        Request.Make("/Data/GetAllClasses/", "get").then(function (res) {
            $scope.classes = res.data;
        });

        // Gets all students from the server //
        Request.Make("/Data/GetAllUsers/", "get", { roleFilter: "Student" }).then(function (res) {
            var tmp = [];

            angular.forEach(res.data, function (value, key) {
                if (value.schoolClassID == null) {
                    tmp.push(value);
                }
            });

            $scope.students = tmp;
        });

        
        // Variables //
        $scope.Add = addToClass;
        $scope.Remove = removeFromClass;
        $scope.SetSelectedClass = setSelectedClass;
        $scope.SetDisabled = setDisabled;
        $scope.class = {};

        $scope.selectedClass;
        $scope.classes = [];
        $scope.students = [];

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/Class/_create.html",
            edit: basePath + "/Class/_edit.html",
            addstudents: basePath + "/Class/_addStudents.html",
            remove: basePath + "/Class/_remove.html",
            form: basePath + "/Class/_form.html"
        };

        // Set form post destination //
        var sendFormTo;
        var popupResponseMessage;
        var toDisable;
        switch ($scope.currentPage) {
            case "create":
                sendFormTo = "/Data/CreateNewSchoolClass/";
                popupResponseMessage = "New school class has been created";
                $scope.submitBtnText = "Create";
                break;
            case "edit":
                sendFormTo = "/Data/UpdateSchoolClass/";
                popupResponseMessage = "School class has been updated";
                toDisable = "input, select, button[type=submit]";
                $scope.submitBtnText = "Update";
                break;
            case "remove":
                sendFormTo = "/Data/RemoveSchoolClass/";
                popupResponseMessage = "School class has been removed";
                toDisable = "button[type=submit]";
                $scope.submitBtnText = "Remove";
                break;
        }

        // Adding new class //
        $scope.SendForm = sendForm;
        $scope.schoolClass = {};
        $scope.dateToday = new Date();
    }

    LMSApp.controller("ClassController", [
        "$scope",
        "Request",
        "Popup",
        "$filter",
        "$routeParams",
        "$route",
        ClassController
    ]);

})();