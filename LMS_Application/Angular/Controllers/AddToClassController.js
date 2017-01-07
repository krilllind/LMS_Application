(function () {

    var AddToClassController = function ($scope, Request, Popup) {

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
    }

    LMSApp.controller("AddToClassController", [
        "$scope",
        "Request",
        "Popup",
        AddToClassController
    ]);

})();