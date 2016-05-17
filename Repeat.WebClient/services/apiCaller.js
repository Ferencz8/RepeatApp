/**
 * Created by Ferencz on 4/14/2016.
 */
var http = require('http');
var config = require('../common/config');

var apiCaller = (function () {

    var host = config.host;

    var getRequest = function (route, cb) {

        var options = {
            host: host,
            path: route
        };

        var callback = function (response) {
            var str = '';
            response.setEncoding('utf8');
            response.on('data', function (chunk) {
                console.log('Data received')
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

    var postRequest = function (route, parameters, cb) {


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

        http.request(postOptions, cb).write(bodyString);
    };

    var putRequest = function (route, parameters, cb) {
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


        // on request response call cb(callback)
        http.request(putOptions, cb).write(bodyString);
    };

    var deleteRequest = function(route, cb){

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