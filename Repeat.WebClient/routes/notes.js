/**
 * Created by Ferencz on 4/14/2016.
 */
var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');

var notesRoute = config.notesRoute;

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
        var editedNote = req.body;
        var route = notesRoute + editedNote.id;
        var getcallback = function (apiNote) {
            apiNote.Name = editedNote.name;
            apiNote.Content = editedNote.content;

            var date = new Date();
            apiNote.ModifiedDate = date.toISOString();
            var putRoute = notesRoute + apiNote.Id;


            var putCallBack = function (putResponse) {

                res.redirect('http://localhost:3000/notebooks/notes?notebookId=' + apiNote.NotebookId);
            };
            apiCaller.putRequest(putRoute, apiNote, putCallBack);
        };

        apiCaller.getRequest(route, getcallback);
    });

router.post('/delete', function (req, res) {
    var route = notesRoute + req.body.id;

    var deleteCallBack = function (deleteResponse) {
        res.redirect('http://localhost:3000/notebooks/notes?notebookId=' + req.body.notebookId);
    };

    apiCaller.deleteRequest(route, deleteCallBack);
});

router.route('/add')
    .get(function (req, res) {
        res.render('partials/notes/addNote', {
            title: 'Add Note',
            notebookId: req.query.notebookId
        });
    })
    .post(function (req, res) {
        var addedNote = req.body;

        var postCallback = function (postResponse) {
            res.redirect('http://localhost:3000/notebooks/notes?notebookId=' + req.body.notebookId);
        };

        apiCaller.postRequest(notesRoute, addedNote, postCallback);
    });
module.exports = router;