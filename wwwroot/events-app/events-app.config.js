'use strict';

angular.
  module('eventsApp').
  config(['$locationProvider', '$routeProvider',
    function config($locationProvider, $routeProvider) {

        $routeProvider.
          when('/events', {
              template: '<event-list></event-list>'
          }).
          when('/events/:eventId', {
              template: '<event-detail></event-detail>'
          }).
          otherwise('/');

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
  ]);
