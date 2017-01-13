(function () {

    var fileDnd = function () {

        var Link = function (scope, element, attributes, ctrl) {
            var onDragOver = function (e) {
                e.preventDefault();

                try {
                    if (containsFiles(e))
                        element.addClass("play");
                    else
                        setNoDrop(e);
                }
                catch (err) {
                    setNoDrop(e);
                };
            };

            var onDragEnd = function (e) {
                e.preventDefault();
                element.removeClass("play");
                element.removeClass("no-drop");
                angular.element(element[0].querySelector('.btn-select-file')).eq(0).removeAttr("disabled");
            };

            var onDrop = function (e) {
                onDragEnd(e);
                scope.UploadFile(e.dataTransfer.files);
            }

            function setNoDrop(e) {
                e.dataTransfer.dropEffect = "none";
                e.dataTransfer.effectAllowed = "none";
                element.addClass("no-drop");
                angular.element(element[0].querySelector('.btn-select-file')).eq(0).attr("disabled", true);
            }

            function containsFiles(event) {
                if (event.dataTransfer.types) {
                    for (var i = 0; i < event.dataTransfer.types.length; i++) {
                        if (event.dataTransfer.types[i] == "Files") {
                            return true;
                        }
                    }
                }

                return false;
            }

            element.bind("dragover", onDragOver)
                  .bind("dragleave", onDragEnd)
                  .bind("drop", onDrop);
        }

        return {
            restrict: "AE",
            link: Link
        };
    }

    LMSApp.directive("fileDnd", [fileDnd]);

}());