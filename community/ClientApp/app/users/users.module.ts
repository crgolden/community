import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";

import { IndexComponent } from "./index/index.component"
import { DetailsComponent } from "./details/details.component"

import { UsersService } from "./users.service"

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        RouterModule.forChild([
            { path: "Users", component: IndexComponent },
            { path: "Users/Details/:id", component: DetailsComponent }
        ])
    ],
    declarations: [
        IndexComponent,
        DetailsComponent
    ],
    providers: [
        UsersService
    ]
})
export class UsersModule {
}