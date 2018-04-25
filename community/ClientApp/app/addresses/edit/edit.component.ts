import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-edit",
    templateUrl: "./edit.component.html",
    styleUrls: ["./edit.component.css"]
})
export class EditComponent implements OnInit {

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
        this.address = this.route.snapshot.data["address"];
    }

    edit(valid: boolean) {
        this.submitted = true;

        if (valid) {
            this.isRequesting = true;
            this.addressesService
                .edit(this.address)
                .finally(() => this.isRequesting = false)
                .subscribe(
                () => this.router.navigate([`/Addresses/Details/${this.address.id}`]),
                (error: string) => this.errors = error);
        }
    }
}
