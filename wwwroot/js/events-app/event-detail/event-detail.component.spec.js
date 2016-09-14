"use strict";

describe("eventDetail", function () {

    // Load the module that contains the `eventDetail` component before each test
    beforeEach(module("eventDetail"));

    // Test the controller
    describe("EventDetailController", function () {
        var $httpBackend, ctrl;
        var xyzEventData = {
            name: "event xyz",
            images: ["image/url1.png", "image/url2.png"]
        };

        beforeEach(inject(function ($componentController, _$httpBackend_, $routeParams) {
            $httpBackend = _$httpBackend_;
            $httpBackend.expectGET("events-data/xyz.json").respond(xyzEventData);

            $routeParams.eventId = "xyz";

            ctrl = $componentController("eventDetail");
        }));

        it("should fetch the event details", function () {
            jasmine.addCustomEqualityTester(angular.equals);
            expect(ctrl.event).toEqual({});

            $httpBackend.flush();
            expect(ctrl.event).toEqual(xyzEventData);
        });

    });

});
