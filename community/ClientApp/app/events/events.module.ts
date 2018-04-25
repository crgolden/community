import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";

import { IndexComponent } from "./index/index.component"
import { DetailsComponent } from "./details/details.component"
import { CreateComponent } from "./create/create.component"
import { EditComponent } from "./edit/edit.component"
import { DeleteComponent } from "./delete/delete.component"

import { EventsService } from "./events.service"
import { AppCanActivate } from "../app.can-activate"
import { IndexResolver } from "./index/index.resolver"
import { DetailsResolver } from "./details/details.resolver"

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forChild([
            {
                path: "Events",
                component: IndexComponent,
                resolve: { events: IndexResolver }
            },
            {
                path: "Events/Details/:id",
                component: DetailsComponent,
                resolve: { event: DetailsResolver }
            },
            {
                path: "Events/Create",
                component: CreateComponent,
                canActivate: [AppCanActivate]
            },
            {
                path: "Events/Edit/:id",
                component: EditComponent,
                resolve: { event: DetailsResolver },
                canActivate: [AppCanActivate]
            },
            {
                path: "Events/Delete/:id",
                component: DeleteComponent,
                resolve: { event: DetailsResolver },
                canActivate: [AppCanActivate]
            }
        ])
    ],
    declarations: [
        IndexComponent,
        DetailsComponent,
        CreateComponent,
        EditComponent,
        DeleteComponent
    ],
    providers: [
        EventsService,
        AppCanActivate,
        IndexResolver,
        DetailsResolver
    ]
})
export class EventsModule {
}