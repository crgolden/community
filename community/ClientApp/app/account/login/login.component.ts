import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AccountService } from "../account.service"
import { ILogin } from "./login.interface"

@Component({
    selector: "app-login-form",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"]
})
export class LoginComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;

    constructor(private readonly accountService: AccountService, private readonly router: Router) { }

    login({ value, valid }: { value: ILogin, valid: boolean }) {

        var that = this;
        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            that.accountService
                .login(value)
                .finally(() => that.isRequesting = false)
                .subscribe((res) => {
                    if (res) {
                        if (typeof value.returnUrl !== "undefined") {
                            that.router.navigate([value.returnUrl]);
                        } else {
                            that.router.navigate(["/"]);
                        }
                    }
                },
                error => that.errors = error);
        }
    }
}
