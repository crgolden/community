import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { IndexComponent } from "./index/index.component"
import { DetailsComponent } from "./details/details.component"
import { CreateComponent } from "./create/create.component"

import { AddressesService } from "./addresses.service"

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forChild([
            { path: 'addresses', component: IndexComponent },
            { path: 'addresses/details/:id', component: DetailsComponent },
            { path: 'addresses/create', component: CreateComponent }
        ])
    ],
    declarations: [
        IndexComponent,
        DetailsComponent,
        CreateComponent
    ],
    providers: [
        AddressesService
    ]
})
export class AddressesModule {
}