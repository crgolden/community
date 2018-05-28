import { NgModule } from "@angular/core";

import { RouterModule } from "@angular/router";
import { AccountModule } from "./account/account.module"
import { UsersModule } from "./users/users.module"
import { EventsModule } from "./events/events.module"
import { AddressesModule } from "./addresses/addresses.module"

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";

import { AppService } from "./app.service"

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent
    ],
    imports: [
        RouterModule.forRoot([
            { path: "", redirectTo: "Home", pathMatch: "full" },
            { path: "Home", component: HomeComponent },
            { path: "**", redirectTo: "Home" }
        ]),
        AccountModule,
        UsersModule,
        EventsModule,
        AddressesModule
    ],
    providers: [
        AppService
    ]
})
export class AppModuleShared {
}
