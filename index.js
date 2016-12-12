var express = require('express');
var path = require('path');
var app = express();
app.use(require("body-parser")());
var handlebars = require('express-handlebars');
var server = require("http").Server(app);
var mysql = require("mysql");
var Validator = require("sql-validator");

// Server set up
const PORT = 80;
server.listen(PORT);

// Simple file routing
app.use(express.static(path.join(__dirname, 'public')));
app.engine('hbs', handlebars({
    defaultLayout: 'main',
    extname: '.hbs'
}));
app.set('view engine', 'hbs');
app.set('views', path.join(__dirname, 'views'));

// mysql stuff
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : 'db_user',
  password : 'bebek123',
  database : 'test'
});

connection.connect(function(err) {
    if(err) {
        console.error("Error connecting to mysql server");
        return;
    }

    console.log("Connected as id " + connection.threadId);
});


// sql validation
var crimeValidator = new Validator({
    select: ['type', 'startDate', 'endDate']
});

var crimeMap = {
    type: crimeValidator.check().dataType("isAlpha"),
    startDate: crimeValidator.check().dataType("isDate"),
    endDate: crimeValidator.check().dataType("isDate")
};

var evidenceValidator = new Validator({
    select: ['id']
});

var evidenceMap = {
    id: evidenceValidator.check().dataType("isNumeric")
};

var vehicleValidator = new Validator({
    select: ['model', 'state']
});

var vehicleMap = {
    model: vehicleValidator.check().dataType("isAlpha"),
    state: vehicleValidator.check().dataType("isAlpha")
};

var officerValidator = new Validator({
    select: ['name']
});

var officerMap = {
    name: officerValidator.check().dataType("isAlpha")
};

// server stuff
app.get('/', function(req, res) {
	res.render('index');
});

app.post('/', function(req, res) {
    if(!req.xhr) {
        return;
    }

    var form = req.body;
    var query = {};
    var stmt = "";

    if(form.formType === "crime") {
        query = {
            type: form.crimeType,
            startDate: new Date(form.startDate),
            endDate: new Date(form.endDate)
        }
        if(crimeValidator.isValid(query, crimeMap, 'select').valid) {
            stmt = "SELECT * FROM crime WHERE "
            + "CrimeType='" + form.crimeType
            + "' AND TimeDate BETWEEN '" + toSQLDateFormat(new Date(form.startDate))
            + "' AND '" + toSQLDateFormat(new Date(form.endDate)) + "';";

            connection.query(stmt, function(err, results) {
                if(err) return;
                res.send(results);
            });
        } else {
            throw "err";
        }
    } else if(form.formType === "evidence") {
        query = {
            id: form.crimeId
        }
        if(evidenceValidator.isValid(query, evidenceMap, 'select').valid) {
            stmt = "SELECT * FROM evidence WHERE "
            + "Case_id=" + form.crimeId + ";";

            connection.query(stmt, function(err, results) {
                if(err) return;
                res.send(results);
            });
        } else {
            throw "err";
        }
    } else if(form.formType === "vehicle") {
        query = {
            model: form.model,
            state: form.state
        }
        if(vehicleValidator.isValid(query, vehicleMap, 'select').valid) {
            stmt = "SELECT * FROM vehicles WHERE "
            + "Model='" + form.model
            + "' AND StateProvince='" + form.state + "';";

            connection.query(stmt, function(err, results) {
                if(err) return;
                res.send(results);
            });
        } else {
            throw "err";
        }
    } else if(form.formType === "officer") {
        query = {
            name: form.name
        }
        if(officerValidator.isValid(query, officerMap, 'select').valid) {
            stmt = "SELECT * FROM officers WHERE "
            + "Name='" + form.name + "';";

            connection.query(stmt, function(err, results) {
                if(err) return;
                res.send(results);
            });
        } else {
            throw "err";
        }
    } else {
        throw "error";
    }
});


// helper functions

// from http://stackoverflow.com/questions/5129624/convert-js-date-time-to-mysql-datetime
function twoDigits(d) {
    if(0 <= d && d < 10) return "0" + d.toString();
    if(-10 < d && d < 0) return "-0" + (-1*d).toString();
    return d.toString();
}

function toSQLDateFormat(date) {
    return date.getUTCFullYear() + "-" + twoDigits(1 + date.getUTCMonth()) + "-" + twoDigits(date.getUTCDate()) + " " + twoDigits(date.getUTCHours()) + ":" + twoDigits(date.getUTCMinutes()) + ":" + twoDigits(date.getUTCSeconds());
};


// error stuff
// catch 404 and forward to error handler
app.use(function (req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
});

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use(function (err, req, res, next) {
        res.status(err.status || 500);
        res.render('error', {
            message: err.message,
            error: err
        });
    });
}

// production error handler
// no stacktraces leaked to user
app.use(function (err, req, res, next) {
    res.status(err.status || 500);
    res.render('error', {
        message: err.message,
        error: {}
    });
});


module.exports = app;