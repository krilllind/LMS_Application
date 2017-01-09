(function () {

    var ClassController = function ($scope, Request, Popup, $filter) {

        var addToClass = function (student) {
            if ($scope.selectedClass) {
                var index = $scope.students.indexOf(student);
                if (index != -1)
                    $scope.students.splice(index, 1);

                Request.Make("/Data/AddStudentsToClass/", "post", JSON.stringify({ classID: $scope.selectedClass.id, studentSSN: student.ssn })).then(function (res) {
                    $scope.selectedClass.students.push(student);
                });
            }
            else {
                Popup.Message("Sorry", "You need to select a school class before you can add students.", Popup.types.info, { confirmText: "Got it!" });
            }
        }

        var removeFromClass = function (student) {
            var index = $scope.selectedClass.students.indexOf(student);
            if (index != -1)
                $scope.selectedClass.students.splice(index, 1);

            Request.Make("/Data/RemmoveStudentsFromClass/", "post", JSON.stringify({ classID: $scope.selectedClass.id, studentSSN: student.ssn })).then(function (res) {
                $scope.students.push(student);
            });
        }

        var setSelectedClass = function (cls) {
            $scope.selectedClass = cls;
        }

        var createNewClass = function (isValid) {
            if (isValid) {
                var schoolClassTmp = angular.copy($scope.schoolClass);
                schoolClassTmp.validTo = $filter('date')(schoolClassTmp.validTo, "yyyy-MM-dd");

                Request.Make("/Account/GetAntiForgeryToken/", "post").then(function (token) {
                    Request.Make("/Data/CreateNewSchoolClass/", "post", JSON.stringify(schoolClassTmp), null, { 'RequestVerificationToken': token.data }).then(function (res) {
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
                            Popup.Message("Created!", "New class " + schoolClassTmp.name + " has been created.", Popup.types.ok, { timer: 5000 }).then(function (res) {
                                $scope.registerForm.$setPristine();
                                $scope.registerForm.$setUntouched();

                                $scope.schoolClass = {}
                            });
                        }
                    });
                });
            }
            else {
                Popup.Message("Sorry", "You need to enter all information before you can create a new class.", Popup.types.error, { confirmText: "Okey" });
            }
        }

        // Gets all classes from the server //
        Request.Make("/Data/GetAllClasses/", "get").then(function (res) {
            $scope.classes = res.data;
        });

        // Gets all students from the server //
        Request.Make("/Data/GetAllStudents/", "get").then(function (res) {
            $scope.students = res.data;
        });

        $scope.Add = addToClass;
        $scope.Remove = removeFromClass;
        $scope.SetClass = setSelectedClass;

        $scope.selectedClass;
        $scope.classes = [];
        $scope.students = [];

        // Adding new class //
        $scope.CreateNewClass = createNewClass;
        $scope.schoolClass;
    }

    LMSApp.controller("ClassController", [
        "$scope",
        "Request",
        "Popup",
        "$filter",
        ClassController
    ]);

})();