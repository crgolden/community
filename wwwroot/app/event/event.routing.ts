import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EventDetailComponent } from './event-detail.component'
import { EventListComponent } from './event-list.component';

export const eventRouting: ModuleWithProviders = RouterModule.forChild([
    {
        path: 'events',
        component: EventListComponent
    },
    {
        path: 'detail/:id',
        component: EventDetailComponent
    }
]);