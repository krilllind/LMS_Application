(function () {

    var AddToClassController = function ($scope, Request) {

        var addToClass = function (student) {
            if ($scope.selectedClass) {
                var index = $scope.students.indexOf(student);

                if (index != -1)
                    $scope.students.splice(index, 1);

                $scope.selectedClass.students.push(student)
                Request.Make("/Data/AddStudentsToClass/", "post", { classID: $scope.selectedClass.ID, studentSSN: student.ssn }).then(function (res) {
                    console.log(res);
                });
            }
            else {
                alert("Please select a class first!");
            }
        }

        var removeFromClass = function (student) {
            var index = $scope.selectedClass.students.indexOf(student);
            if (index != -1)
                $scope.selectedClass.students.splice(index, 1);

            $scope.students.push(student);
        }

        var setSelectedClass = function (cls) {
            $scope.selectedClass = cls;
        }

        // Gets all classes from the server //
        Request.Make("/Data/GetAllClasses/", "get").then(function (res) {
            $scope.classes = res.data;
            console.log(res);
        });

        // Gets all students from the server //
        Request.Make("/Data/GetAllStudents/", "get").then(function (res) {
            $scope.students = res.data;
            console.log(res);
        });

        $scope.Add = addToClass;
        $scope.Remove = removeFromClass;
        $scope.SetClass = setSelectedClass;

        $scope.selectedClass;
        $scope.classes = [];
        $scope.students = [];

        //$scope.classes = [
        //    {
        //        ID: "sddfgdgfdgdf",
        //        name: "TE6B",
        //        students: []
        //    },
        //    {
        //        ID: "sddfgsdhfggfdgdf",
        //        name: "NA3E",
        //        students: []
        //    },
        //    {
        //        ID: "dfhgth5y34ertre",
        //        name: "BF1A",
        //        students: []
        //    },
        //    {
        //        ID: "uer7e8yueryhr87he",
        //        name: "BF1B",
        //        students: []
        //    }
        //];

        //$scope.students = [
        //    {
        //        firstname: "Lars",
        //        lastname: "Gunnarsson",
        //        ssn: "943793834895"
        //    },
        //    {
        //        firstname: "Jomi",
        //        lastname: "Gunnarsson",
        //        ssn: "89546875487"
        //    },
        //    {
        //        firstname: "Linnéa",
        //        lastname: "Andersson",
        //        ssn: "2134213443"
        //    },
        //    {
        //        firstname: "Fia",
        //        lastname: "Nilsson",
        //        ssn: "883488478"
        //    },
        //];
    }

    LMSApp.controller("AddToClassController", [
        "$scope",
        "Request",
        AddToClassController
    ]);

})();