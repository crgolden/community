//jshint strict: false
module.exports = function (config) {
    config.set({

        files: [
          "wwwroot/lib/angular/angular.js",
          "wwwroot/lib/angular-route/angular-route.js",
          "wwwroot/lib/angular-resource/angular-resource.js",
          "wwwroot/lib/angular-mocks/angular-mocks.js",
          "events-app/**/*.module.js",
          "events-app/**/*!(.module|.spec).js",
          "events-app/**/*.spec.js"
        ],

        autoWatch: true,

        frameworks: ["jasmine"],

        browsers: ["Chrome"],

        plugins: [
          "karma-chrome-launcher",
          "karma-jasmine"
        ],

        reporters: [
            "progress"
        ]
    });
};
