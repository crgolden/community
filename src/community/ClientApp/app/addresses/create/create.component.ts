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
    model = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router) {
        this.model.userId = this.addressesService.getUser().id;
    }

    create(valid: boolean) {
        if (valid) {
            this.addressesService
                .create(this.model)
                .subscribe(
                    (address: string | Address) => {
                        if (typeof address !== "string") {
                            this.router.navigate([`/Addresses/Details/${address.id}`]);
                        }
                    },
                    (error: string) => this.errors = error);
        }
    }
}
