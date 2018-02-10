import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-create",
    templateUrl: "./create.component.html",
    styleUrls: ["./create.component.css"]
})
export class CreateComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly location: Location) { }

    saveAddress({ value, valid }: { value: Address, valid: boolean }) {

        var that = this;

        if (valid) {
            that.submitted = true;
            that.isRequesting = true;
            value.userId = that.addressesService.user.id;
            that.addressesService
                .createAddress(value)
                .finally(() => that.isRequesting = false)
                .subscribe(res => {
                    if (typeof res.id !== "undefined" && res.id.length > 0) {
                        that.router.navigate([`/addresses/details/${res.id}`]);
                    }
                },
                error => that.errors = error);
        }
    }

    goBack(): void {
        this.location.back();
    }
}
