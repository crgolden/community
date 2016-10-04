import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { SharedModule } from '../shared/shared.module';

import { EventDetailComponent } from './event-detail.component';
import { EventListComponent } from './event-list.component';
import { EventSearchComponent } from './event-search.component';
import { EventService } from './event.service';
import { eventRouting } from './event.routing';

@NgModule({
    imports: [
        SharedModule,
        HttpModule,
        eventRouting
    ],
    declarations: [
        EventDetailComponent,
        EventListComponent,
        EventSearchComponent
    ],
    providers: [
        EventService
    ] 
})
export class EventModule { }