import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from '@angular/router';

import { AccountService } from "./account.service";

import { RegisterComponent } from "./register/register.component";
import { LoginComponent } from "./login/login.component";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            { path: 'account/register', component: RegisterComponent },
            { path: 'account/login', component: LoginComponent }
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