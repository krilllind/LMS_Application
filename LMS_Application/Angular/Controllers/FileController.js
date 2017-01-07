(function () {

    var FileController = function ($scope, Request) {

        var displayImage = function (filename) {
            Request.Make("/File/GetUrlByFilename/", "get", filename).then(function (res) {
                $scope.url = res.data;
            });

            $scope.buttonClicked = true;
        }

        var uploadFile = function (files) {
            var fd = new FormData();

            for (var i = 0; i < files.length; i++) {
                console.log(files[i]);
                fd.append(files[i].name, files[i]);
            }

            Request.Make("/File/UploadFiles/", "post", fd).then(function (res) {
                console.log(res.data);
            });
        }

        var allFileNames = function () {
            Request.Make("/File/GetAllFilenames/", "get").then(function (res) {
                console.log(res.data);
            });
        }

        $scope.AllFileNames = allFileNames;
        $scope.DisplayImage = displayImage;
        $scope.UploadFile = uploadFile;
        $scope.FilesToUpload = [];
        $scope.buttonClicked = false;
        $scope.url = "";
    }

    LMSApp.controller('FileController', [
        '$scope',
        'Request',
        FileController
    ]);

}());
