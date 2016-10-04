import './rxjs-extensions';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { routing } from './app.routing';

import { EventModule } from './event/event.module'

@NgModule({
    imports: [
        BrowserModule,
        EventModule,
        FormsModule,
        routing
    ],
    declarations: [
        AppComponent,
        DashboardComponent
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {
}
