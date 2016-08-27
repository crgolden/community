//jshint strict: false
module.exports = function(config) {
    config.set({

        basePath: './wwwroot',

        files: [
          'lib/angular/angular.js',
          'lib/angular-route/angular-route.js',
          'lib/angular-resource/angular-resource.js',
          'lib/angular-mocks/angular-mocks.js',
          'events-app/**/*.module.js',
          'events-app/*!(.module|.spec).js',
          '!(lib)/**/*!(.module|.spec).js',
          'events-app/**/*.spec.js'
        ],

        autoWatch: true,

        frameworks: ['jasmine'],

        browsers: ['Chrome'],

        plugins: [
          'karma-chrome-launcher',
          'karma-jasmine'
        ],

        reporters: [
            'progress'
        ]

});
};
