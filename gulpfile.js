/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
"use strict";

var clean = require('gulp-clean');
var gulp = require("gulp");
var Server = require("karma").Server;
var ts = require('gulp-typescript');
var destPath = './wwwroot/lib/';

gulp.task("default", function () {
});

/**
 * Run test once and exit
 */

gulp.task("test", function (done) {
    new Server({
        configFile: __dirname + "/karma.conf.js",
        singleRun: true
    }, done).start();
});
