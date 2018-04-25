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

import { AddressesService } from "./addresses.service"
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
                path: "Addresses",
                component: IndexComponent,
                resolve: { addresses: IndexResolver }
            },
            {
                path: "Addresses/Details/:id",
                component: DetailsComponent,
                resolve: { address: DetailsResolver }
            },
            {
                path: "Addresses/Create",
                component: CreateComponent,
                canActivate: [AppCanActivate]
            },
            {
                path: "Addresses/Edit/:id",
                component: EditComponent,
                resolve: { address: DetailsResolver },
                canActivate: [AppCanActivate]
            },
            {
                path: "Addresses/Delete/:id",
                component: DeleteComponent,
                resolve: { address: DetailsResolver },
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
        AddressesService,
        AppCanActivate,
        IndexResolver,
        DetailsResolver
    ]
})
export class AddressesModule {
}