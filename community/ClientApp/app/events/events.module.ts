import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { IndexComponent } from "./index/index.component"
import { DetailsComponent } from "./details/details.component"
import { CreateComponent } from "./create/create.component"

import { EventsService } from "./events.service"

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forChild([
            { path: 'events', component: IndexComponent },
            { path: 'events/details/:id', component: DetailsComponent },
            { path: 'events/create', component: CreateComponent }
        ])
    ],
    declarations: [
        IndexComponent,
        DetailsComponent,
        CreateComponent
    ],
    providers: [
        EventsService
    ]
})
export class EventsModule {
}