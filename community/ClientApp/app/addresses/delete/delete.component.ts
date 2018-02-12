import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-delete",
    templateUrl: "./delete.component.html",
    styleUrls: ["./delete.component.css"]
})
export class DeleteComponent {

    errors: string = "";
    address = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        var that = this;
        const id = this.route.snapshot.paramMap.get("id");

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

    delete() {
        var that = this;

        if (typeof that.address.id == "string" && that.address.id.length === 36) {
            that.addressesService.delete(that.address.id)
                .subscribe(
                    res => {
                        if (typeof res == "boolean" && res) {
                            that.router.navigate(["/Addresses"]);
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
