(function () {

    var ScheduleController = function ($scope, Request, Schedule, $routeParams) {
        var canvas = document.getElementById("ScheduleCanvas");
        var spinner = document.getElementById("canvasLoading");

        canvas.style.opacity = 0;

        Request.Make("/Account/GetAntiForgeryToken/", "get").then(function (token) {
            Request.Make("/Data/GetSchedule/", "get", null, null, { 'RequestVerificationToken': token.data }).then(function (res) {
                Schedule.Initialize(canvas, 5, false, "08:00", "17:00", res.data);

                spinner.style.opacity = 0;
                canvas.style.opacity = 1;
            });
        });

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "create").toLowerCase();
        $scope.template = {
            create: basePath + "/Schedule/_create.html",
            edit: basePath + "/Schedule/_edit.html",
            remove: basePath + "/Schedule/_remove.html"
        };
    }

    LMSApp.controller('ScheduleController', [
        '$scope',
        'Request',
        'Schedule',
        '$routeParams',
        ScheduleController
    ]);

}());