(function () {

    var FileController = function ($scope, Request, Popup, $filter, $routeParams) {

        var displayImage = function (filename) {
            Request.Make("/File/GetUrlByFilename/", "get", filename).then(function (res) {
                $scope.url = res.data;
            });

            $scope.buttonClicked = true;
        }

        var uploadFile = function (files) {
            var fd = new FormData();

            angular.forEach(files, function (value, key) {
                fd.append("file" + key, value);
            });

            Request.Make("/File/UploadFiles/", "post", fd, null, { "Content-Type": undefined }, angular.identity).then(function (res) {
                if (res.status.ok) {
                    Request.Make("/File/GetUrlByFilename/", "get", { fileName: files[0].name } ).then(function (blob) {
                        $scope.blobUrl = blob.data;
                    });
                }
            });
        }

        var allFileNames = function () {
            Request.Make("/File/GetAllFilenames/", "get").then(function (res) {
                console.log(res.data);
            });
        }

        var openFileExplorer = function () {
            var fileDialog = document.getElementById("fileUploadDrop").querySelector(".dnd-upload");
            var evt = document.createEvent("MouseEvents");
            evt.initEvent("click", true, false);
            fileDialog.dispatchEvent(evt);
        }

        // Get current subpage //
        $scope.currentPage = ($routeParams.handle || "upload").toLowerCase();
        $scope.template = {
            upload: basePath + "/File/_upload.html",
            download: basePath + "/File/_download.html",
            edit: basePath + "/File/_edit.html",
            remove: basePath + "/File/_remove.html"
        };

        // Set form post destination //
        var sendFormTo;
        switch ($scope.currentPage) {
            case "upload":
                sendFormTo = "/File/UploadFiles/";
                $scope.submitBtnText = "Upload";
                break;
            case "download":
                sendFormTo = "/File/DownloadFiles/";
                $scope.submitBtnText = "Download";
                break;
            case "edit":
                sendFormTo = "File/UpdateFile/";
                $scope.submitBtnText = "Update";
                break;
            case "remove":
                sendFormTo = "/File/RemoveFile/";
                $scope.submitBtnText = "Remove";
                break;
        }

        $scope.OpenFileExplorer = openFileExplorer;
        $scope.AllFileNames = allFileNames;
        $scope.DisplayImage = displayImage;
        $scope.UploadFile = uploadFile;
        $scope.FilesToUpload = [];
        $scope.buttonClicked = false;
        $scope.url = "";
        $scope.blobUrl;
    }

    LMSApp.controller('FileController', [
        "$scope",
        "Request",
        "Popup",
        "$filter",
        "$routeParams",
        FileController
    ]);

}());
