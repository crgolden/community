import './rxjs-extensions';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard.component';
import { EventsComponent } from './events.component';
import { EventDetailComponent } from './event-detail.component';
import { EventSearchComponent } from './event-search.component';
import { EventService } from './event.service';
import { routing } from './app.routing';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing
    ],
    declarations: [
        AppComponent,
        DashboardComponent,
        EventDetailComponent,
        EventSearchComponent,
        EventsComponent
    ],
    providers: [
        EventService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
