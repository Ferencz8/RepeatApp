var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');
var authorization = require('../common/authorization');

var notebooksRoute = config.notebooksRoute;
/* GET home page. */


router.get('/', authorization.authorize, function (req, res) {

    var renderCallBack = function (apiNotebooks) {

        var localNotebooks = removeDeleted(apiNotebooks);
        res.locals = {
            title: 'List of Notebooks',
            notebooks: localNotebooks,
            username: req.cookies.username
        };

        res.render('layouts/sharedLayout', {
            //title: apiNotebook.Name,
            //notes: apiNotes,
            //notebookId: apiNotebook.Id
        });
    };

    apiCaller.getRequest(notebooksRoute, {'Authorization': req.cookies.token}, renderCallBack);
});

router.route('/add', authorization.authorize)
    .get(function (req, res) {

        res.render('addNotebook', {
            title: 'Add Notebook'
        });
    })
    .post(function (req, res) {

        var addedNotebook = req.body;

        addedNotebook.UserId = req.cookies.userId;
        var postCallback = function (postResponse) {
            res.redirect('/notebooks');
        };

        apiCaller.postRequest(notebooksRoute, addedNotebook, {'Authorization': req.cookies.token}, postCallback);
    });

router
    .get('/edit', authorization.authorize, function (req, res) {
        var route = notebooksRoute + req.query.notebookId;
        var getCallback = function (apiNotebook) {

            res.render('editNotebook', {
                title: 'Edit Notebook',
                Id: apiNotebook.Id,
                Name: apiNotebook.Name
            });
        };
        apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getCallback);

    })
    .post('/edit', authorization.authorize, function (req, res) {
        var editedNotebook = req.body;
        var route = notebooksRoute + editedNotebook.id;
        var getcallback = function (apiNotebook) {
            apiNotebook.Name = editedNotebook.name;

            var date = new Date();
            apiNotebook.ModifiedDate = date.toISOString();
            var putRoute = notebooksRoute + apiNotebook.Id;


            var putCallBack = function (putResponse) {

                res.redirect('/notebooks');
            };
            apiCaller.putRequest(putRoute, apiNotebook, {'Authorization': req.cookies.token}, putCallBack);
        };

        apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);
    });

router.post('/delete', authorization.authorize, function (req, res) {
    var route = notebooksRoute + req.body.id;
    var getcallback = function (apiNotebook) {

        var date = new Date();
        apiNotebook.ModifiedDate = date.toISOString();
        apiNotebook.Deleted = true;
        apiNotebook.DeletedDate = date.toISOString();
        var putRoute = notebooksRoute + apiNotebook.Id;


        var putCallBack = function (putResponse) {

            res.send(putResponse.statusCode);
        };
        apiCaller.putRequest(putRoute, apiNotebook, {'Authorization': req.cookies.token}, putCallBack);
    };

    apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getcallback);
});

router.get('/getNotebookById', authorization.authorize, function (req, res) {

    var route = notebooksRoute + req.query.notebookId;
    var renderCallBack = function (apiNotebook) {

        res.render('editNotebook', {
            title: 'Edit Notebook',
            notebook: apiNotebook
        });
        //res.send({title: "EditNotebook", notebook: apiNotebook})
    };

    apiCaller.getRequest(route, renderCallBack);
});

router.get('/notes', authorization.authorize, function (req, res) {

    var route = notebooksRoute + req.query.notebookId + "/notes";
    var renderCallBack = function (apiNotes) {

        apiNotes = removeDeleted(apiNotes);
        res.render('notes/notes2', {
            Id: req.query.notebookId,
            Name: req.query.notebookName,
            notes: apiNotes
        });
    };

    apiCaller.getRequest(route, {'Authorization': req.cookies.token}, renderCallBack);
});

function removeDeleted(list) {
    var cleanedList = [];
    for (var x in list) {
        if (list[x].Deleted != true) {
            cleanedList.push(list[x]);
        }
    }
    return cleanedList;
}

module.exports = router;
