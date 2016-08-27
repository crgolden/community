'use strict';

// Register `eventDetail` component, along with its associated controller and template
angular.
  module('eventDetail').
  component('eventDetail', {
      templateUrl: 'app/event-detail/event-detail.template.html',
      controller: ['$routeParams', 'Event',
        function EventDetailController($routeParams, Event) {
            var self = this;
            self.event = Event.get({ eventId: $routeParams.eventId }, function (event) {
                self.setImage(event.images[0]);
            });
            self.setImage = function setImage(imageUrl) {
                self.mainImageUrl = imageUrl;
            };
        }
      ]
  });
