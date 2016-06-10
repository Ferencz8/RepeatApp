/**
 * Created by Ferencz on 6/3/2016.
 */
var UserApp = require("userapp");
var config = require("../common/config");
var User = require("../models/User");

UserApp.initialize({
    appId: config.userAPIKey
});

var authenticationService = (function (){
    function login (username, password, successcallback, errorcallback){

        UserApp.User.login({"login": username, "password": password}, function(error, result) {
            if (error) {
                // Something went wrong...
                // Check error.name. Might just be a wrong password?

            } else {
                // User is logged in, save result.token in session
                if(successcallback != undefined){
                    User.token = result.token;
                    User.userId = result.user_id;
                    successcallback(User);
                }
            }
        });
    }

    function logout (callback){
        UserApp.User.logout(callback);
    }

    function signup(){
        //TO BE ADDED
    }

    return {
        login: login,
        logout: logout,
        signup: signup
    }
})();

module.exports = authenticationService;