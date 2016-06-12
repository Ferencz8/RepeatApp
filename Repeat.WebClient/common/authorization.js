/**
 * Created by Ferencz on 6/12/2016.
 */
var authenticationService = require('../services/authenticationService');
var authorization = {
    authorize: function (req, res, next) {
        if (req.cookies != undefined && req.cookies.token != undefined) {
            //authenticationService.authorize(req.cookies.token, function () {
            //
            //    next();
            //}, function(){
            //    res.redirect("/");
            //});

            //authenticationService.refreshToken(req.cookies.token, function(){
            //    next();
            //}, function(){
            //    res.redirect("/");
            //})
            next();
        } else {
            res.redirect("/");
        }
    }
};

module.exports = authorization;