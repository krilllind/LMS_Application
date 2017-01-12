(function () {
    var RequestProvider = function ($http) {

        // If an error occurs display it in the console
        var OnError = function (response) {
            console.error("RequestProvider Error >> " + response.status + " >> (" + response.statusText + ")");

            return {
                data: null,
                message: response.statusText,
                status: {
                    ok: (response.status <= 201) ? true : false,
                    error: (response.status > 201) ? true : false,
                    code: response.status
                }
            };
        }

        // Returns the data obj from server
        var make = function (TO, TYPE, DATA, ENCTYPE, HEADERS) {
            TO = TO || null;
            TYPE = TYPE || null;
            DATA = DATA || null;
            var PARAMS = DATA || null;

            if (TYPE == "post")
                PARAMS = {};

            HEADERS = HEADERS || { "Content-Type": "application/x-www-form-urlencoded" };
            ENCTYPE = ENCTYPE || null;

            if (!TO) {
                OnError({
                    status: 500,
                    statusText: "Invalid Action string {" + TO + "}"
                });
            }

            if (!((String(TYPE).toLowerCase() != "get") || (String(TYPE).toLowerCase() != "post"))) {
                OnError({
                    status: 500,
                    statusText: "Invalid type {" + TYPE + "}"
                });
            }

            return $http({
                url: TO,
                method: TYPE,
                enctype: ENCTYPE,
                params: PARAMS,
                data: DATA,
                headers: HEADERS,
                transformRequest: angular.identity
            }).then(function (response) {
                return {
                    data: response.data,
                    message: response.statusText,
                    status: {
                        ok: (response.status <= 201) ? true : false,
                        error: (response.status > 201) ? true : false,
                        code: response.status
                    }
                };
            }, OnError);
        }

        return {
            Make: make
        };
    }

    LMSApp.factory("Request", [
        "$http",
        RequestProvider
    ]);

}());