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
    address = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router) {
        this.address.userId = this.addressesService.getUser().id;
    }

    create(valid: boolean) {
        this.submitted = true;

        if (valid) {
            this.isRequesting = true;
            this.addressesService
                .create(this.address)
                .finally(() => this.isRequesting = false)
                .subscribe(
                (id: string) => this.router.navigate([`/Addresses/Details/${id}`]),
                (error: string) => this.errors = error);
        }
    }
}
