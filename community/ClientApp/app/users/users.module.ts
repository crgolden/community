import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";

import { IndexComponent } from "./index/index.component"
import { DetailsComponent } from "./details/details.component"

import { UsersService } from "./users.service"
import { IndexResolver } from "./index/index.resolver"
import { DetailsResolver } from "./details/details.resolver"

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        RouterModule.forChild([
            {
                path: "Users",
                component: IndexComponent,
                resolve: { users: IndexResolver }
            },
            {
                path: "Users/Details/:id",
                component: DetailsComponent,
                resolve: { user: DetailsResolver }
            }
        ])
    ],
    declarations: [
        IndexComponent,
        DetailsComponent
    ],
    providers: [
        UsersService,
        IndexResolver,
        DetailsResolver
    ]
})
export class UsersModule {
}