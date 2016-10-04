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
var http_1 = require('@angular/http');
var shared_module_1 = require('../shared/shared.module');
var event_detail_component_1 = require('./event-detail.component');
var event_list_component_1 = require('./event-list.component');
var event_search_component_1 = require('./event-search.component');
var event_service_1 = require('./event.service');
var event_routing_1 = require('./event.routing');
var EventModule = (function () {
    function EventModule() {
    }
    EventModule = __decorate([
        core_1.NgModule({
            imports: [
                shared_module_1.SharedModule,
                http_1.HttpModule,
                event_routing_1.eventRouting
            ],
            declarations: [
                event_detail_component_1.EventDetailComponent,
                event_list_component_1.EventListComponent,
                event_search_component_1.EventSearchComponent
            ],
            providers: [
                event_service_1.EventService
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], EventModule);
    return EventModule;
}());
exports.EventModule = EventModule;
//# sourceMappingURL=event.module.js.map