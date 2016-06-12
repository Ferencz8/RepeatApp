/**
 * Created by Ferencz on 6/3/2016.
 */
var UserApp = require("userapp");
var config = require("../common/config");
var User = require("../models/User");

UserApp.initialize({
    appId: config.userAPIKey
});

var authenticationService = (function () {
    function login(username, password, successcallback, errorcallback) {

        UserApp.User.login({"login": username, "password": password}, function (error, result) {
            if (error) {
                // Something went wrong...
                // Check error.name. Might just be a wrong password?
                if (errorcallback) {
                    errorcallback(error.message);
                }
            } else {
                // User is logged in, save result.token in session
                if (successcallback != undefined) {


                    User.token = result.token;
                    User.userId = result.user_id;

                    UserApp.User.get({}, function (error, result) {

                        if (error) {
                            console.log(error);
                            return;
                        }

                        var user = result[0];
                        User.username = user.login;
                        successcallback(User);
                    });
                }
            }
        });
    }

    function logout(callback) {
        UserApp.User.logout(callback);
    }

    function signup(username, email, password, successcallback, errorcallback) {

        UserApp.User.save({"login": username, "password": password, "email": email}, function (error, result) {
            if (error) {
                // Something went wrong...
                // Check error.name. Might just be a wrong password?

            } else {
                // User is logged in, save result.token in session
                if (successcallback != undefined) {

                    successcallback();
                }
            }
        });
    }

    function authorize(token, authorizedCallback, notAuthorizedCalback) {
        UserApp.initialize({
            appId: config.userAPIKey,
            token: token
        });

        UserApp.User.get({}, function (error, result) {

            if (error) {
                console.log(error);

                //for demo purposes
                if(error.name=='INTERNAL_ERROR'){
                    authorizedCallback();
                }

                if(notAuthorizedCalback){
                    notAuthorizedCalback();
                }
            }

            if(authorizedCallback) {
                authorizedCallback();
            }
        });
    }

    function refreshToken(token, succesfullRefreshCb, failedCb){
        UserApp.Token.heartbeat(function(error, result){
            // Handle error/result
            if(error != undefined || result.alive == false){
                failedCb();
            }
            else if(result.alive = true){
                succesfullRefreshCb();
            }
        });
    }

    return {
        login: login,
        logout: logout,
        signup: signup,
        authorize: authorize,
        refreshToken: refreshToken,
    }
})();

module.exports = authenticationService;