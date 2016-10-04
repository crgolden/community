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
var event_1 = require('../event/event');
var EventDetailComponent = (function () {
    function EventDetailComponent(eventService, route) {
        this.eventService = eventService;
        this.route = route;
    }
    EventDetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route.params.forEach(function (params) {
            var id = +params['id'];
            _this.eventService.getEvent(id)
                .then(function (event) { return _this.event = event; });
        });
    };
    EventDetailComponent.prototype.goBack = function () {
        window.history.back();
    };
    EventDetailComponent.prototype.save = function () {
        var _this = this;
        this.eventService.update(this.event)
            .then(function () { return _this.goBack(); });
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', event_1.Event)
    ], EventDetailComponent.prototype, "event", void 0);
    EventDetailComponent = __decorate([
        core_1.Component({
            selector: 'my-event-detail',
            templateUrl: './app/event-detail/event-detail.component.html',
            styleUrls: ['./app/event-detail/event-detail.component.css']
        }), 
        __metadata('design:paramtypes', [event_service_1.EventService, router_1.ActivatedRoute])
    ], EventDetailComponent);
    return EventDetailComponent;
}());
exports.EventDetailComponent = EventDetailComponent;
//# sourceMappingURL=event-detail.component.js.map