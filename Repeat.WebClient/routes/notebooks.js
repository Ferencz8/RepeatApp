var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');

var notebooksRoute = config.notebooksRoute;
/* GET home page. */
router.get('/', function (req, res) {

    var renderCallBack = function (apiNotebooks) {

        res.locals = {
            title: 'List of Notebooks',
            notebooks: apiNotebooks
        };
        res.render('index');
    };

    apiCaller.getRequest(notebooksRoute, renderCallBack);
});

router.route('/add')
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

router.post('/edit', function (req, res) {

    var editedNotebook = req.body;
    var route = notebooksRoute + editedNotebook.id;
    var getcallback = function (apiNotebook) {
        apiNotebook.Name = editedNotebook.name;

        var date = new Date();
        apiNotebook.ModifiedDate = date.toISOString();
        var putRoute = notebooksRoute + apiNotebook.Id;


        var putCallBack = function (putResponse) {

            res.redirect('http://localhost:3000/notebooks');
        };
        apiCaller.putRequest(putRoute, apiNotebook, putCallBack);
    };

    apiCaller.getRequest(route, getcallback);
});

router.post('/delete', function (req, res) {

    var route = notebooksRoute + req.body.id;

    var deleteCallBack = function (deleteResponse) {
        res.redirect('http://localhost:3000/notebooks');
    };

    apiCaller.deleteRequest(route, deleteCallBack);
});

router.get('/getNotebookById', function (req, res) {

    var route = notebooksRoute + req.query.noteBookId;
    var renderCallBack = function (apiNotebook) {

        res.render('editNotebook', {
            title: 'Edit Notebook',
            notebook: apiNotebook
        });
        //res.send({title: "EditNotebook", notebook: apiNotebook})
    };

    apiCaller.getRequest(route, renderCallBack);
});

router.get('/notes', function (req, res) {
    var route = notebooksRoute + req.query.notebookId + "/notes";
    var renderCallBack = function (apiNotes) {
        var redirect = !req.xhr;
        if (redirect) {
            apiCaller.getRequest(notebooksRoute + req.query.notebookId, function callback(apiNotebook) {

                res.render('layouts/notesContainer', {
                    title: 'Notes of ' + apiNotebook.Name,
                    notes: apiNotes,
                    notebookId: apiNotebook.Id
                });
            });
        } else {

            res.render('partials/notes/notes', {
                title: 'Notes of ' + req.query.notebookName,
                notes: apiNotes,
                notebookId: req.query.notebookId
            });
        }
    };

    apiCaller.getRequest(route, renderCallBack);
});

module.exports = router;
