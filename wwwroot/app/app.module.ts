import './rxjs-extensions';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SharedModule } from './shared/shared.module'
import { routing } from './app.routing';

import { EventModule } from './event/event.module'

@NgModule({
    imports: [
        BrowserModule,
        EventModule,
        SharedModule,
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
