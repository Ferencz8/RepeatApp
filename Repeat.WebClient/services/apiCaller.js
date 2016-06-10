/**
 * Created by Ferencz on 4/14/2016.
 */
var http = require('http');
var config = require('../common/config');

var apiCaller = (function () {

    var host = config.host;

    var getRequest = function (route, headers, cb) {

        var options = {
            host: host,
            path: route,
            headers: headers
        };

        var callback = function (response) {
            var str = '';
            response.setEncoding('utf8');
            response.on('data', function (chunk) {
                console.log('Data received');
                str += chunk;
            });
            response.on('end', function () {

                var strAsJSON = JSON.parse(str);
                cb(strAsJSON);
            });
        };

        http.get(options, callback).on('error', function (e) {
            console.log('problem with request: ' + e.message);
        });
    };

    var postRequest = function (route, parameters, headers, cb) {


        var bodyString = JSON.stringify(parameters);

        var postOptions = {
            host: host,
            path: route,
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Content-Length': bodyString.length
            }
        };

        for (var propertyName in headers) {
            postOptions.headers.propertyName = headers[propertyName];
        }

        http.request(postOptions, cb).write(bodyString);
    };

    var putRequest = function (route, parameters, headers, cb) {
        var bodyString = JSON.stringify(parameters);

        var putOptions = {
            host: host,
            path: route,
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
                'Content-Length': bodyString.length
            }
        };

        for (var propertyName in headers) {
            putOptions.headers.propertyName = headers[propertyName];
        }
        // on request response call cb(callback)
        http.request(putOptions, cb).write(bodyString);
    };

    var deleteRequest = function (route, headers, cb) {

        var deleteOptions = {
            host: host,
            path: route,
            method: "DELETE"
        };

        http.request(deleteOptions, cb).end();
    };

    return {
        getRequest: getRequest,
        postRequest: postRequest,
        putRequest: putRequest,
        deleteRequest: deleteRequest
    };
})();

module.exports = apiCaller;