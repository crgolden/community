import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { EventsComponent } from '../events/events.component';
import { EventDetailComponent } from '../event-detail/event-detail.component';
import { EventSearchComponent } from '../event-search/event-search.component';
import { EventService } from '../event/event.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpModule
    ],
    declarations: [
        EventsComponent,
        EventDetailComponent,
        EventSearchComponent
    ],
    exports: [
        EventsComponent
    ],
    providers: [
        EventService
    ] 
})
export class EventModule { }