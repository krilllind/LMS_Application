(function () {

    var image = function () {

        var bind = function (scope, element, attributes) {
            scope.$watch("bgImage", function (url) {
                if (angular.isDefined(url)) {
                    element.css({
                        'background-image': 'url(' + url + ')',
                        'background-size': 'cover'
                    });
                }
            });
        };

        return {
            restrict: "A",
            scope: { bgImage: "=" },
            link: bind
        };

    }

    LMSApp.directive("bgImage", [image]);

}());