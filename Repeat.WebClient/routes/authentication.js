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

        if (postData.username == undefined && postData == undefined) {
            //return login error
        }
        else {
            authenticationService.login(postData.username, postData.password, function (user) {
                res.cookie('token', user.token);
                res.cookie('userId', user.userId);
                //TODO:: add authorization for each route
                res.redirect('/notebooks');
            });
        }
    });

router.route('/logout')
    .get(function (req, res) {
        res.clearCookie('token');

        res.redirect('/');
    });

router.route('/signup')
    .get(function (req, res) {

        res.render('addNotebook', {
            title: 'Add Notebook'
        });
    })
    .post(function (req, res) {

        var addedNotebook = req.body;

        var postCallback = function (postResponse) {
            res.redirect('http://localhost:3000/notebooks');
        };

        apiCaller.postRequest(notebooksRoute, addedNotebook, postCallback);
    });

module.exports = router;
