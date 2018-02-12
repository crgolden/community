import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AccountService } from "../account.service"
import { IRegister } from "./register.interface"

@Component({
    selector: "register",
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.css"]
})
export class RegisterComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;

    constructor(
        private readonly accountService: AccountService,
        private readonly router: Router) {
    }

    register({ value, valid }: { value: IRegister, valid: boolean }) {

        var that = this;

        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            that.accountService
                .register(value)
                .finally(() => that.isRequesting = false)
                .subscribe(
                    (res: boolean | string) => {
                        debugger;
                        if (typeof res == "boolean" && res)
                            if (typeof that.accountService.getReturnUrl() == "string") {
                                that.router.navigate([that.accountService.getReturnUrl()]);
                                that.accountService.setReturnUrl("");
                            } else {
                                that.router.navigate(["/Home"]);
                            }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
