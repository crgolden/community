"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var event_service_1 = require('../event/event.service');
var EventsComponent = (function () {
    function EventsComponent(router, eventService) {
        this.router = router;
        this.eventService = eventService;
    }
    EventsComponent.prototype.getEvents = function () {
        var _this = this;
        this.eventService.getEvents().then(function (events) { return _this.events = events; });
    };
    EventsComponent.prototype.ngOnInit = function () {
        this.getEvents();
    };
    EventsComponent.prototype.onSelect = function (event) {
        this.selectedEvent = event;
    };
    EventsComponent.prototype.gotoDetail = function () {
        this.router.navigate(['/detail', this.selectedEvent.id]);
    };
    EventsComponent.prototype.add = function (title) {
        var _this = this;
        title = title.trim();
        if (!title) {
            return;
        }
        this.eventService.create(title)
            .then(function (event) {
            _this.events.push(event);
            _this.selectedEvent = null;
        });
    };
    EventsComponent.prototype.delete = function (event) {
        var _this = this;
        this.eventService
            .delete(event.id)
            .then(function () {
            _this.events = _this.events.filter(function (h) { return h !== event; });
            if (_this.selectedEvent === event) {
                _this.selectedEvent = null;
            }
        });
    };
    EventsComponent = __decorate([
        core_1.Component({
            selector: 'my-events',
            templateUrl: './app/events/events.component.html',
            styleUrls: ['./app/events/events.component.css']
        }), 
        __metadata('design:paramtypes', [router_1.Router, event_service_1.EventService])
    ], EventsComponent);
    return EventsComponent;
}());
exports.EventsComponent = EventsComponent;
//# sourceMappingURL=events.component.js.map