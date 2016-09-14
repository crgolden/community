"use strict";

describe("Event", function() {
  var $httpBackend;
  var Event;
  var eventsData = [
    {name: "Event X"},
    {name: "Event Y"},
    {name: "Event Z"}
  ];

  // Add a custom equality tester before each test
  beforeEach(function() {
    jasmine.addCustomEqualityTester(angular.equals);
  });

  // Load the module that contains the `Event` service before each test
  beforeEach(module("core.event"));

  // Instantiate the service and "train" `$httpBackend` before each test
  beforeEach(inject(function(_$httpBackend_, _Event_) {
    $httpBackend = _$httpBackend_;
    $httpBackend.expectGET("events-data/events.json").respond(eventsData);

    Event = _Event_;
  }));

  // Verify that there are no outstanding expectations or requests after each test
  afterEach(function () {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });

  it("should fetch the events data from `events-data/events.json`", function() {
    var events = Event.query();

    expect(events).toEqual([]);

    $httpBackend.flush();
    expect(events).toEqual(eventsData);
  });

});
