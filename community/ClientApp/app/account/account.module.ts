import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

import { AccountService } from "./account.service";

import { RegisterComponent } from "./register/register.component";
import { LoginComponent } from "./login/login.component";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            { path: "Account/Register", component: RegisterComponent },
            { path: "Account/Login", component: LoginComponent }
        ])
    ],
    declarations: [
        RegisterComponent,
        LoginComponent
    ],
    providers: [
        AccountService
    ]
})
export class AccountModule {
}