'use strict';

describe('eventList', function () {

    // Load the module that contains the `eventList` component before each test
    beforeEach(module('eventList'));

    // Test the controller
    describe('EventListController', function () {
        var $httpBackend, ctrl;

        // The injector ignores leading and trailing underscores here (i.e. _$httpBackend_).
        // This allows us to inject a service and assign it to a variable with the same name
        // as the service while avoiding a name conflict.
        beforeEach(inject(function ($componentController, _$httpBackend_) {
            $httpBackend = _$httpBackend_;
            $httpBackend.expectGET('../events-app/events/events.json')
                        .respond([{ name: 'Nexus S' }, { name: 'Motorola DROID' }]);

            ctrl = $componentController('eventList');
        }));

        it('should create a `events` property with 2 events fetched with `$http`', function () {
            jasmine.addCustomEqualityTester(angular.equals);

            expect(ctrl.events).toEqual([]);

            $httpBackend.flush();
            expect(ctrl.events).toEqual([{ name: 'Nexus S' }, { name: 'Motorola DROID' }]);
        });

        it('should set a default value for the `orderProp` property', function () {
            expect(ctrl.orderProp).toBe('age');
        });

    });

});
