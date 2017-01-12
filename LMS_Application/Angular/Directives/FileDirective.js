(function () {

    var fileDnd = function () {

        var bind = function (scope, element, attributes, ctrl) {
            console.log("hej");

            element.on('dragover, dragenter', function (e) {
                console.log(e);

                e.preventDefault();
                e.stopPropagation();
            });

            //element.on('drop', function (e) {
            //    e.preventDefault();
            //    e.stopPropagation();

            //    if (e.originalEvent.dataTransfer) {
            //        if (e.originalEvent.dataTransfer.files.length > 0) {
            //            ctrl.fileDnd = e.originalEvent.dataTransfer.files;
            //            console.log(ctrl.fileDnd);
            //        }
            //    }

            //    return false;
            //});
        }

        return {
            restrict: "E",
            require: "ngModel",
            link: bind
        };
    }

    LMSApp.directive("fileDnd", [fileDnd]);

}());