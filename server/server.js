var express = require('express');
var http = require('http');
var socketIO = require('socket.io');

var app = express();
var server = http.Server(app);
server.listen(5000);

var io = socketIO(server);

var min = 0;
var max = 99;
var NumberGuess = Math.floor(Math.random() * (max - min)) + min;

const leaderBoard = [];
leaderBoard.length = 3;

leaderBoard[0] = { name: 'xxx', countReply: 999 };
leaderBoard[1] = { name: 'xxxx', countReply: 9999 };
leaderBoard[2] = { name: 'xxxxx', countReply: 99999 };

var test = { name: 'newlusifer', countReply: 5 };

io.on('connection', function (socket) {
    console.log('client connected');
    console.log("The number guess is " + NumberGuess);

    socket.emit('startGuess');

    console.log('Number 1 :' + leaderBoard[0].name + ' with ' + leaderBoard[0].countReply + ' reply');
    console.log('Number 2 :' + leaderBoard[1].name + ' with ' + leaderBoard[1].countReply + ' reply');
    console.log('Number 3 :' + leaderBoard[2].name + ' with ' + leaderBoard[2].countReply + ' reply');

    socket.on('reply', function (data) {
        console.log(data.name + " : " + data.reply + " //" + data.countReply + " reply");//message คือ key ที่ต้องการ

        if (data.reply > NumberGuess) {
            socket.emit('than', { status: 'That Than Number' });
        }

        if (data.reply < NumberGuess) {
            socket.emit('less', { status: 'That Less Number' });
        }

        if (data.reply == NumberGuess)//have winner
        {
            socket.broadcast.emit('winner', { name: data.name });
            socket.emit('winner', { name: data.name });
            NumberGuess = Math.floor(Math.random() * (max - min)) + min;//random number
            console.log("The number guess is " + NumberGuess);

            if (data.countReply <= leaderBoard[0].countReply) {

                if (data.name == leaderBoard[0].name) {
                    //still
                    leaderBoard[0].countReply = data.countReply;
                }

                if (data.name != leaderBoard[0].name) {
                    if (leaderBoard[0].countReply <= leaderBoard[1].countReply) {
                        if (leaderBoard[1].countReply <= leaderBoard[2].countReply) {
                            leaderBoard[2] = leaderBoard[1];
                        }

                        leaderBoard[1] = leaderBoard[0];
                    }

                    leaderBoard[0] = { name: data.name, countReply: data.countReply };

                }

            }

            else if (data.countReply <= leaderBoard[1].countReply) {


                if (data.name == leaderBoard[1].name) {
                    //still
                    leaderBoard[1].countReply = data.countReply;
                }

                if (data.name != leaderBoard[1].name) {

                    if (data.name == leaderBoard[0].name) {
                        //still                   
                    }

                    if (data.name != leaderBoard[0].name) {
                        //still    

                        if (leaderBoard[1].countReply <= leaderBoard[2].countReply) {
                            leaderBoard[2] = leaderBoard[1];
                        }
                        leaderBoard[1] = { name: data.name, countReply: data.countReply };
                    }


                }
            }

            else if (data.countReply <= leaderBoard[2].countReply) {

                if (data.name == leaderBoard[2].name) {
                    //still
                    leaderBoard[2].countReply = data.countReply;
                }

                if (data.name != leaderBoard[2].name) {
                    if (data.name == leaderBoard[0].name || data.name == leaderBoard[1].name) {
                        //still                   
                    }

                    if (data.name != leaderBoard[0].name || data.name != leaderBoard[1].name) {
                        leaderBoard[2] = { name: data.name, countReply: data.countReply };
                    }


                }
            }

            socket.emit('sendLeaderBoard1', leaderBoard[0]);
            socket.emit('sendLeaderBoard2', leaderBoard[1]);
            socket.emit('sendLeaderBoard3', leaderBoard[2]);

            console.log('Number 1 :' + leaderBoard[0].name + ' with ' + leaderBoard[0].countReply + ' reply');
            console.log('Number 2 :' + leaderBoard[1].name + ' with ' + leaderBoard[1].countReply + ' reply');
            console.log('Number 3 :' + leaderBoard[2].name + ' with ' + leaderBoard[2].countReply + ' reply');
        }

    });//end on reply   

    socket.on('wantLeaderBoard1', function (data) {
        socket.emit('sendLeaderBoard1', leaderBoard[0]);
        // console.log('send Leaderboard1');
    });//end on wantLeaderBoard  

    socket.on('wantLeaderBoard2', function (data) {
        socket.emit('sendLeaderBoard2', leaderBoard[1]);
        //console.log('send Leaderboard2');
    });//end on wantLeaderBoard  

    socket.on('wantLeaderBoard3', function (data) {
        socket.emit('sendLeaderBoard3', leaderBoard[2]);
        // console.log('send Leaderboard3');
    });//end on wantLeaderBoard  


});

console.log('server started');