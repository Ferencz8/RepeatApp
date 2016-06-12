/**
 * Created by Ferencz on 4/14/2016.
 */
var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');

var authorization = require('../common/authorization');
var notesRoute = config.notesRoute;

router.get('/getById', authorization.authorize, function (req, res) {

    var route = notesRoute + req.query.noteId;

    var getcallback = function (apiNote) {
        res.send(apiNote);
    };

    apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);
});

router.route('/edit', authorization.authorize)
    .get(function (req, res) {
        var route = notesRoute + req.query.noteId;
        var renderCallBack = function (apiNote) {

            res.render('partials/notes/editNote', {
                title: 'Edit Note',
                note: apiNote
            });
        };

        apiCaller.getRequest(route, renderCallBack);
    })
    .post(function (req, res) {

        var editedNote = req.body;
        var route = notesRoute + editedNote.id;
        var getcallback = function (apiNote) {
            apiNote.Name = editedNote.name;
            apiNote.Content = editedNote.content;

            var date = new Date();
            apiNote.ModifiedDate = date.toISOString();
            var putRoute = notesRoute + apiNote.Id;


            var putCallBack = function (putResponse) {
                res.sendStatus(204);
            };
            apiCaller.putRequest(putRoute, apiNote, {'Authorization': req.cookies.token}, putCallBack);
        };

        apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);

    });

router.post('/delete', authorization.authorize, function (req, res) {

    var route = notesRoute + req.body.id;

    var getcallback = function (apiNote) {

        var date = new Date();
        apiNote.ModifiedDate = date.toISOString();
        apiNote.Deleted = true;
        apiNote.DeletedDate = date.toISOString();
        var putRoute = notesRoute + apiNote.Id;

        var putCallBack = function (putResponse) {

            res.sendStatus(204);
        };
        apiCaller.putRequest(putRoute, apiNote, {'Authorization': req.cookies.token}, putCallBack);
    };

    apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);
});

router.route('/add', authorization.authorize)
    .post(function (req, res) {
        var addedNote = req.body;

        addedNote.userId = req.cookies.userId;
        var postCallback = function (postResponse) {
            res.sendStatus(204);
        };

        apiCaller.postRequest(notesRoute, addedNote, {'Authorization': req.cookies.token}, postCallback);

    });
module.exports = router;