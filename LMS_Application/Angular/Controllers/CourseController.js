(function () {

    var CourseController = function ($scope, $routeParams, Request) {

        //Request.Make("/Data/GetCourses/", "get", $routeParams.courseName || null).then(function (data) {
        //    console.log(data);
        //});
    }

    LMSApp.controller("CourseController", [
        "$scope",
        "$routeParams",
        "Request",
        CourseController
    ]);

})();