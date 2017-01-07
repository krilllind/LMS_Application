(function () {

    var FR = function () {

        var load = function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                scope.filereader = changeEvent.target.files;
            });
        }

        return {
            scope: {
                filereader: "="
            },
            link: load
        };
    }

    LMSApp.directive("filereader", [FR]);

}());