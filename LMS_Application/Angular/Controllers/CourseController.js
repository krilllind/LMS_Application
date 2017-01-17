(function () {

    var CourseController = function ($scope, Request, Popup, $filter, $routeParams, $route) {

        Request.Make("/Data/GetAllMyCourses/", "get", $routeParams.courseName || null).then(function (data) {
            console.log(data.data);
            $scope.courses = data.data;
        });

        // Variables //
        $scope.courses = [];

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "mycourses").toLowerCase();
        $scope.template = {
            mycourses: basePath + "/Course/_mycourses.html",
            assignments: basePath + "/Course/_assignments.html",
            join: basePath + "/Course/_join.html",
            leave: basePath + "/Course/_leave.html"
        };
    }

    LMSApp.controller("CourseController", [
        "$scope",
        "Request",
        "Popup",
        "$filter",
        "$routeParams",
        "$route",
        CourseController
    ]);

})();