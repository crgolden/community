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
    model = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.model = this.route.snapshot.data["address"];
    }

    edit(valid: boolean) {
        if (valid) {
            this.addressesService
                .edit(this.model)
                .subscribe(
                    () => this.router.navigate([`/Addresses/Details/${this.model.id}`]),
                    (error: string) => this.errors = error);
        }
    }
}
