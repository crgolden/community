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
    isRequesting: boolean = false;
    submitted: boolean = false;
    addresses: Address[];

    constructor(private readonly addressesService: AddressesService, private readonly router: Router) { }

    ngOnInit(): void {

        var that = this;
        
        that.addressesService
            .getAddresses()
            .subscribe(addresses => {
                    that.addresses = addresses;
                },
                error => that.errors = error);
    }

}
