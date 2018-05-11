import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AccountService } from "../account.service"
import { Login } from "./login"

@Component({
    selector: "login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"]
})
export class LoginComponent {

    errors: string = "";
    model = new Login();

    constructor(
        private readonly accountService: AccountService,
        private readonly router: Router) {
    }

    login(valid: boolean) {
        if (valid) {
            this.accountService
                .login(this.model)
                .subscribe(
                    (res: boolean | string) => {
                        if (typeof res === "boolean" && res) {
                            const returnUrl = this.accountService.getReturnUrl();
                            if (typeof returnUrl === "string") {
                                this.accountService.setReturnUrl("");
                                this.router.navigate([returnUrl]);
                            } else {
                                this.router.navigate(["/Home"]);
                            }
                        }
                    },
                    (error: string) => this.errors = error);
        }
    }
}
