/**
 * Created by Ferencz on 4/14/2016.
 */
var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');

var notesRoute = config.notesRoute;

router.get('/getById', function (req, res) {
    if (req.cookies.token == undefined) {
        res.redirect('/');
    }
    else {
        var route = notesRoute + req.query.noteId;

        var getcallback = function (apiNote) {
            res.send(apiNote);
        };

        apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);
    }
});

router.route('/edit')
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
        if (req.cookies.token == undefined) {
            res.redirect('/');
        }
        else {
            //TODO:: check for sql injection
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
        }
    });

router.post('/delete', function (req, res) {
    if (req.cookies.token == undefined) {
        res.redirect('/');
    }
    else {
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
    }
});

router.route('/add')
    //.get(function (req, res) {
    //    res.render('partials/notes/addNote', {
    //        title: 'Add Note',
    //        notebookId: req.query.notebookId
    //    });
    //})
    .post(function (req, res) {
        if (req.cookies.token == undefined) {
            res.redirect('/');
        }
        else {
            var addedNote = req.body;

            addedNote.userId = req.cookies.userId;
            var postCallback = function (postResponse) {
                //TODO:: check postResponse.statusCode()
                res.sendStatus(204);
            };

            apiCaller.postRequest(notesRoute, addedNote, {'Authorization': req.cookies.token}, postCallback);
        }
    });
module.exports = router;