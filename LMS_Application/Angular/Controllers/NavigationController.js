﻿(function () {

    var NavigationController = function ($scope, Request, $window, Popup) {

        var SearchFiler = function () {
            // TODO: Hitta alla kurser, lärare mm  som matchar sökresultatet. (Akronym?)
        }

        var LogOut = function () {
            Popup.Message("Logout", "Are you sure you want to logout?", Popup.types.warning, { enableCancel: true }).then(function (res) {
                console.log(res);
                if (res === true) {
                    Request.Make("/Account/LogOff/", "post").then(function (res) {
                        if (res.status.ok) {
                            $window.location.reload();
                        }
                    });
                }
            });
        }

        Request.Make("/Data/GetUserInformation/", "get").then(function (res) {
            console.log(res);
            $scope.user = res.data[0];
            $scope.user.userRole = ((res.data[0].userRole === "Teacher") ? true : false);
        });

        $scope.$watch("searchText", SearchFiler);
        $scope.LogOut = LogOut;
        $scope.searchText = "";
        $scope.user = null;
    }

    LMSApp.controller("NavigationController", [
        "$scope",
        "Request",
        "$window",
        "Popup",
        NavigationController
    ]);

}());