var express = require('express');
var router = express.Router();
var apiCaller = require('../services/apiCaller');
var config = require('../common/config');

var notebooksRoute = config.notebooksRoute;
/* GET home page. */
router.get('/', function (req, res) {

    if (req.cookies.token == undefined) {
        res.redirect('/');
    }
    else { //TODO::token should be checked
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
    }
});

router.route('/add')
    .get(function (req, res) {

        res.render('addNotebook', {
            title: 'Add Notebook'
        });
    })
    .post(function (req, res) {
        if (req.cookies.token == undefined) {
            res.redirect('/');
        }
        else {
            var addedNotebook = req.body;

            addedNotebook.UserId = req.cookies.userId;
            var postCallback = function (postResponse) {
                res.redirect('/notebooks');
            };

            apiCaller.postRequest(notebooksRoute, addedNotebook, {'Authorization': req.cookies.token}, postCallback);
        }
    });

router
    .get('/edit', function (req, res) {
        if (req.cookies.token == undefined) {
            res.redirect('/');
        }
        else {
            var route = notebooksRoute + req.query.notebookId;
            var getCallback = function (apiNotebook) {

                res.render('editNotebook', {
                    title: 'Edit Notebook',
                    Id: apiNotebook.Id,
                    Name: apiNotebook.Name
                });
            };
            apiCaller.getRequest(route, {'Authorization': req.cookies.token}, getCallback);
        }
    })
    .post('/edit', function (req, res) {
        if (req.cookies.token == undefined) {
            res.redirect('/');
        }
        else {

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
        }
    });

router.post('/delete', function (req, res) {
    if (req.cookies.token == undefined) {
        res.redirect('/');
    }
    else {

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
    }
});

router.get('/getNotebookById', function (req, res) {

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

router.get('/notes', function (req, res) {
    if (req.cookies.token == undefined) {
        res.redirect('/');
    }
    else { //TODO::token should be checked
        var route = notebooksRoute + req.query.notebookId + "/notes";
        var renderCallBack = function (apiNotes) {
            //var redirect = !req.xhr;
            //if (redirect) {
            //    apiCaller.getRequest(notebooksRoute + req.query.notebookId, function callback(apiNotebook) {
            //
            //        apiNotes = removeDeleted(apiNotes);
            //
            //        res.render('layouts/notesContainer', {
            //            title: 'Notes of ' + apiNotebook.Name,
            //            notes: apiNotes,
            //            notebookId: apiNotebook.Id
            //        });
            //    });
            //} else {
            apiNotes = removeDeleted(apiNotes);

            //res.render('partials/notes/notes', {
            //    title: 'Notes of ' + req.query.notebookName,
            //    notes: apiNotes,
            //    notebookId: req.query.notebookId
            //});
            res.render('notes/notes2', {
                Id: req.query.notebookId,
                Name: req.query.notebookName,
                notes: apiNotes
            });
            //}
        };

        apiCaller.getRequest(route, {'Authorization': req.cookies.token}, renderCallBack);
    }
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
