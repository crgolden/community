'use strict';

// Register `eventList` component, along with its associated controller and template
angular.
  module('eventList').
  component('eventList', {
      templateUrl: 'app/event-list/event-list.template.html',
      controller: ['Event',
          function EventListController(Event) {
              this.events = Event.query();
              this.orderProp = 'age';
          }
      ]
  });
