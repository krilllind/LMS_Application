(function () {

    var xref = function ($location, $route) {
        var bind = function (scope, el, attr) {
            el.on("click", function () {
                scope.$apply(function () {
                    if ($location.url() == attr.xref) {
                        $route.reload();
                    }
                    else {
                        $location.path(attr.xref);
                    }
                });
            });
        }

        return {
            link: bind
        }
    }

    LMSApp.directive("xref", [
        "$location",
        "$route",
        xref
    ]);

}());