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

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forChild([
            { path: "Addresses", component: IndexComponent },
            { path: "Addresses/Details/:id", component: DetailsComponent },
            { path: "Addresses/Create", component: CreateComponent, canActivate: [AppCanActivate] },
            { path: "Addresses/Edit/:id", component: EditComponent, canActivate: [AppCanActivate] },
            { path: "Addresses/Delete/:id", component: DeleteComponent, canActivate: [AppCanActivate] }
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
        AppCanActivate
    ]
})
export class AddressesModule {
}