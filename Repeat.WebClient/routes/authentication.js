/**
 * Created by Ferencz on 6/3/2016.
 */
var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var authenticationService = require('../services/authenticationService');
var config = require('../common/config');

var notebooksRoute = config.notebooksRoute;
/* GET home page. */
router.route('/')
    .get(function (req, res) {

        res.render('index2');
    });

router.route('/login')
    .post(function (req, res) {

        var postData = req.body;

        if (postData.username == undefined || postData == undefined) {
            //return login error
            res.redirect('/', {
                error: 'Username or Password are empty'
            });
        }

        else {
            authenticationService.login(postData.username, postData.password, function (user) {
                res.cookie('token', user.token,{ expires: new Date(Date.now() + 1000 * 60 * 59)});
                res.cookie('userId', user.userId,{ expires: new Date(Date.now() + 1000 * 60 * 59)});
                res.cookie('username', user.username,{ expires: new Date(Date.now() + 1000 * 60 * 59)});
                res.redirect('/notebooks');
            }, function(errorMessage){
                res.render('index2', {
                    error: errorMessage
                });
            });
        }
    })
;

router.route('/logout')
    .get(function (req, res) {
        res.clearCookie('token');

        res.redirect('/');
    });

router.route('/signup')
    .post(function (req, res) {

        var postData = req.body;
        if (postData.username == undefined || postData.password == undefined) {
            res.render('index2');//Check how to display error message
        }
        else {

            authenticationService.signup(postData.username, postData.email, postData.password, function (user) {

                res.redirect('/');
            });
        }
    });

module.exports = router;
