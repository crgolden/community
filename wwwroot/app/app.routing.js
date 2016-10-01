"use strict";
var router_1 = require('@angular/router');
var events_component_1 = require('./events.component');
var event_detail_component_1 = require('./event-detail.component');
var dashboard_component_1 = require('./dashboard.component');
var appRoutes = [
    {
        path: '',
        redirectTo: 'events',
        pathMatch: 'full'
    },
    {
        path: 'events',
        component: events_component_1.EventsComponent
    },
    {
        path: 'dashboard',
        component: dashboard_component_1.DashboardComponent
    },
    {
        path: 'detail/:id',
        component: event_detail_component_1.EventDetailComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map