"use strict";

angular.
  module("core.event").
  factory("Event", ["$resource",
    function($resource) {
      return $resource("api/events", {}, {
        query: {
          method: "GET",
          params: {},
          isArray: true
        }
      });
    }
  ]);
