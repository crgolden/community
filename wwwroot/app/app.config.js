'use strict';

angular.
  module('app').
  config(['$locationProvider', '$routeProvider',
    function config($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');

        $routeProvider.
          when('/events', {
              template: '<event-list></event-list>'
          }).
          when('/events/:eventId', {
              template: '<event-detail></event-detail>'
          }).
          otherwise('/');
    }
  ]);
