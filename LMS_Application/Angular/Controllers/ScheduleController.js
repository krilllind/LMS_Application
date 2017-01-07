(function () {

    var ScheduleController = function ($scope, Request, Schedule) {
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
    }

    LMSApp.controller('ScheduleController', [
        '$scope',
        'Request',
        'Schedule',
        ScheduleController
    ]);

}());