import { Component } from "@angular/core";
import { Router } from "@angular/router";

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
    value = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router) {
    }

    create({ value, valid }: { value: Address, valid: boolean }) {
        var that = this;
        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            value.userId = that.addressesService.getUser().id;
            that.addressesService.create(value)
                .finally(
                    () => that.isRequesting = false)
                .subscribe(
                    (address: Address[] | Address | string) => {
                        if (that.addressesService.isAddress(address)) {
                            that.router.navigate([`/Addresses/Details/${address.id}`]);
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
