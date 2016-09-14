//jshint strict: false
exports.config = {

    allScriptsTimeout: 11000,

    specs: [
      "*.js"
    ],

    rootElement: "[ng-app]",

    capabilities: {
        'browserName': "chrome"
    },

    baseUrl: "http://localhost:5000/",

    framework: "jasmine",

    jasmineNodeOpts: {
        defaultTimeoutInterval: 30000
    },

    seleniumAddress: "http://localhost:4444/wd/hub"

};
