import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent {

    errors: string = "";
    addresses = new Array<Address>();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router) {
    }

    ngOnInit(): void {
        var that = this;

        that.addressesService.index()
            .subscribe(
                (addresses: Address[] | string) => {
                    if (addresses instanceof Array) {
                        that.addresses = addresses;
                    }
                },
                (error: string) => that.errors = error);
    }
}
