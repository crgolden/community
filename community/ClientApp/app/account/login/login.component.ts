import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AccountService } from "../account.service"
import { ILogin } from "./login.interface"

@Component({
    selector: "login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"]
})
export class LoginComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;

    constructor(
        private readonly accountService: AccountService,
        private readonly router: Router) {
    }

    login({ value, valid }: { value: ILogin, valid: boolean }) {

        var that = this;

        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            that.accountService
                .login(value)
                .finally(() => that.isRequesting = false)
                .subscribe(
                    (res: boolean | string) => {
                        if (typeof res == "boolean" && res) {
                            if (typeof that.accountService.getReturnUrl() == "string") {
                                that.router.navigate([that.accountService.getReturnUrl()]);
                                that.accountService.setReturnUrl("");
                            } else {
                                that.router.navigate(["/Home"]);
                            }
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
