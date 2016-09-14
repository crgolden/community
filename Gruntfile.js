/// <binding />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
"use strict";
module.exports = function (grunt) {
    // load Grunt plugins from NPM
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks('grunt-ts');

    // configure plugins
    grunt.initConfig({
        ts: {
            base: {
                src: ['Scripts/events-app/boot.ts', 'Scripts/app/**/*.ts'],
                outDir: 'wwwroot/',
                tsconfig: './tsconfig.json'
            }
        },
        
        uglify: {
            my_target: {
                files: {'wwwroot/events-app.min.js': [
                        "events-app/**/*.module.js",
                        "events-app/**/*.js",
                        "!events-app/**/*.spec.js"
                    ]
                }
            }
        },

        watch: {
            scripts: {
                files: ["Scripts/**/*.js"],
                tasks: ["ts", "uglify"]
            }
        }
    });

    // define tasks
    grunt.registerTask("default", ["ts", "uglify", "watch"]);
};