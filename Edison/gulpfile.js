/// <binding BeforeBuild='' AfterBuild='post-build-copy, kill-processes, deploy, restore-packages' Clean='clean' />
"use strict";
var gulp = require('gulp');
var config = require('./config.json');
var Client = require('ssh2').Client;
var conn;
// JS hint task
// this is just a nice library for making sure your JavaScript syntax is all good
//gulp.task('jshint', function () {
//    var jshint = require('gulp-jshint');
//    gulp.src('./*.js')
//    .pipe(jshint())
//    .pipe(jshint.reporter('default'));
//});

// Copy additional files to bin directory that don't come over as part of the TypeScript build
gulp.task('post-build-copy', function () {
    return gulp.src(['app.js', 'RemoteDebug.js', 'README.md', '*.json'])
        .pipe(gulp.dest('bin'));
});

// deploy to the device
// NOTE: this will only deploy files at the root project level; it is not recursive
gulp.task('deploy', function () {
    var scp = require('gulp-scp2');
    return gulp.src(['bin/*.{js,json,md}'])
        .pipe(scp({
        host: config.host,
        username: config.user,
        password: config.password,
        dest: config.projectName
    }))
        .on('error', function (err) {
        console.log('ERR: ' + err);
    });
});

// Remotely clean the Edison device as well if doing a full project clean
gulp.task('clean', function () {
    conn = new Client();
    conn.on('ready', function () {
        conn.exec('cd ~/' + config.projectName + '; rm --force --recursive *', execCallback);
    }).connect({ host: config.host, port: config.sshPort, username: config.user, password: config.password });
});

//run npm install on the remote machine to assure all packages
gulp.task('restore-packages', function () {
    conn = new Client();
    conn.on('ready', function () {
        conn.exec('cd ~/' + config.projectName + '; npm install --production', execCallback);
    }).connect({ host: config.host, port: config.sshPort, username: config.user, password: config.password });
});

//kill processes
gulp.task('kill-processes', function () {
    conn = new Client();
    conn.on('ready', function () {
        //var path = config.projectName + '/' + config.startFile;
        var path = config.startFile;
        conn.exec('kill -9 `ps | grep "' + path + '" | grep -v grep | awk \' { print $1 }\'`', execCallback);
    }).connect({ host: config.host, port: config.sshPort, username: config.user, password: config.password });
});

//set startup
gulp.task('set-startup', function () {
    var serviceText = "[Unit]\n" +
        "    Description = Node startup app service for starting a node process\n" +
        "    After = mdns.service\n" +
        "[Service]\n" +
        "    ExecStart = /usr/bin/node /home/" + config.user + "/" + config.projectName + "/" + config.startFile + "\n" +
        "    Restart = on-failure\n" +
        "    RestartSec = 2s\n" +
        "[Install]\n" +
        "    WantedBy=default.target\n";
    conn = new ssh2Client();
    conn.on('ready', function () {
        conn.exec('systemctl stop nodeup.service; echo "' + serviceText + '" > /etc/systemd/system/nodeup.service; systemctl daemon-reload; systemctl enable nodeup.service; systemctl start nodeup.service', execCallback);
    }).connect({ host: config.host, port: config.sshPort, username: config.user, password: config.password });
});

//DON'T USE YET
////execute
//gulp.task('execute', ['killProcesses'], function () {
//    conn = new ssh2Client();
//    conn.on('ready', function () {
//        conn.exec("node /home/" + config.user + "/" + config.projectName + "/" + config.startFile, execCallback);
//    }).connect({ host: config.host, port: config.sshPort, username: config.user, password: config.password });
//});

function execCallback(err, stream) {
    if (err)
        throw err;
    stream
        .on('close', function (code, signal) { console.log('Stream closed with code ' + code + ' and signal ' + signal); conn.end(); })
        .on('data', function (data) { console.log(data); })
        .stderr.on('data', function (err) { console.log('Error: ' + err); });
}
//# sourceMappingURL=gulpfile.js.map