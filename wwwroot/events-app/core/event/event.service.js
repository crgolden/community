'use strict';

angular.
  module('core.event').
  factory('Event', ['$resource',
    function($resource) {
      return $resource('../events-app/events/:eventId.json', {}, {
        query: {
          method: 'GET',
          params: {eventId: 'events'},
          isArray: true
        }
      });
    }
  ]);
