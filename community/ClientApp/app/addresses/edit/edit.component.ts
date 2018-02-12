import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-edit",
    templateUrl: "./edit.component.html",
    styleUrls: ["./edit.component.css"]
})
export class EditComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;
    address = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        var that = this;
        const id = that.route.snapshot.paramMap.get("id");

        if (typeof id == "string" && id.length === 36) {
            that.addressesService.details(id)
                .subscribe(
                    (address: Address[] | Address | string) => {
                        if (that.addressesService.isAddress(address)) {
                            that.address.id = address.id;
                            that.address.street = address.street;
                            that.address.street2 = address.street2;
                            that.address.city = address.city;
                            that.address.state = address.state;
                            that.address.zipCode = address.zipCode;
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }

    edit({ value, valid }: { value: Address, valid: boolean }) {
        var that = this;
        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            that.address.street = value.street;
            that.address.street2 = value.street2;
            that.address.city = value.city;
            that.address.state = value.state;
            that.address.zipCode = value.zipCode;
            that.addressesService.edit(that.address)
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
