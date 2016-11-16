// includes library
var net = require('net');
var http = require('http');
var express = require('express');
var path = require('path');

// Http port for connetion
const HTTP_PORT = '3000';
// socket port for connetion
const UNITY_PORT = '8000';

var app = express();
var clients_unity = [];

// boolean used to scene doors
var door_finish = false;
var humanoid_finish = false;

// if user request for home page.
app.get('/', function (req, res) {
    if (req == "portes")
        res.sendFile(path.join(__dirname, '../client/templates', '/portes.html'));
    else
        res.sendFile(path.join(__dirname, '../client/templates', '/menuWeb.html'));
});


app.use(express.static(__dirname + "/.."));
// start webserver
app.listen(HTTP_PORT);

// create and start net socket
var server = net.createServer(function (socket) {

    // add socket to array of net sockets.
    clients_unity.push(socket);

    // event when socket is closed.
    socket.on('close', function (e) {
        clients_unity.splice(clients_unity.indexOf(socket), 1);
    });

    // event if new socket is connecting
    socket.on('data', function (data) {
        if (data.toString() === "door_finish") {
            door_finish = true;
        }
        if (data.toString() === "humanoid_finish") {
            humanoid_finish = true;
        }
    });

    process.on('uncaughtException', function (err) {});

    getAndSendWithoutParams('avatar');
    getAndSendWithoutParams('exit');
    getAndSendWithoutParams('stop');
    getAndSendWithoutParams('M_avatar');
    getAndSendWithoutParams('F_avatar');

    requestDoorsFinish();
    requestHumanoidesFinish();

    getAndSendWithParams('e');
    getAndSendWithParams('db');
    getAndSendWithParams('dh');
    getAndSendWithParams('oob');
    getAndSendWithParams('validerAvatar');
    getAndSendWithParams('hu');

}).listen(UNITY_PORT);

// function to request web user when scene doors is finished
function requestDoorsFinish() {
    app.get('/porte', function (req, res) {
        if (door_finish == true) {
            door_finish = false;
            res.end();
        } else {
            res.sendStatus(403);
        }
    });
}

function requestHumanoidesFinish()
{
    app.get('/humanoide', function (req, res) {
        if (humanoid_finish == true) {
            humanoid_finish = false;
            res.end();
        } else {
            res.sendStatus(403);
        }
    });
}

// create event if receive http get request url without parameters to communique with unity
function getAndSendWithoutParams(url) {
    app.get('/' + url, function (req, res) {
        clients_unity[0].write(url);
        res.end();
    });
}

// create event if receive http get request with parameters to communique with unity
function getAndSendWithParams(url) {
    url += "/:values";
    app.get('/' + url, function (req, res) {
        var send = req.originalUrl.replace('/', '');
        clients_unity[0].write(send);
        res.end();
    });
}
