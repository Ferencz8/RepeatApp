var express = require('express');
var path = require('path');
var notebookRoutes = require('./routes/notebooks');
var notesRoutes = require('./routes/notes');
var authenticationRoutes = require('./routes/authentication');
var app = express();
var cookieParser = require('cookie-parser');
//var cookieSession = require('cookie-session');
//
//app.set('trust proxy', 1); // trust first proxy
//
//app.use(cookieSession({
//    name: 'session',
//    keys: ['token']
//}));


app.use(cookieParser());


app.use(express.static('public'));
app.set('views', path.join(__dirname, 'views'));


//view engine
var handlebars = require('express-handlebars');
var hbs = handlebars.create({
    extname: '.hbs',
    partialsDir: [
        //'views/partials/notes',
        'views/notes'
    ]
});
app.engine('.hbs', hbs.engine);
app.set('view engine', '.hbs');

var bodyParser = require('body-parser')
app.use(bodyParser.json());       // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({     // to support URL-encoded bodies
    extended: true
}));


//redirect / -> /authentication
app.get('/', function (req, res) {
    //res.redirect('http://localhost:3000/notebooks');
    res.redirect('http://localhost:3000/authentication');
    console.log('redirected');
});


app.use('/notebooks', notebookRoutes);
app.use('/notes', notesRoutes);
app.use('/authentication', authenticationRoutes);

//start server
app.listen(3000, function () {
    console.log('express-handlebars example services listening on: 3000');
});