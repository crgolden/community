import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AccountService } from "../account.service"
import { Register } from "./register"

@Component({
    selector: "register",
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.css"]
})
export class RegisterComponent {

    errors: string = "";
    model = new Register();

    constructor(
        private readonly accountService: AccountService,
        private readonly router: Router) {
    }

    register(valid: boolean) {
        if (valid) {
            this.accountService
                .register(this.model)
                .subscribe(
                    (res: boolean | string) => {
                        if (typeof res === "boolean" && res) {
                            const returnUrl = this.accountService.getReturnUrl();
                            if (typeof returnUrl == "string") {
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
