import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-delete",
    templateUrl: "./delete.component.html",
    styleUrls: ["./delete.component.css"]
})
export class DeleteComponent implements OnInit {

    errors: string = "";
    address = new Address();

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.address = this.route.snapshot.data["address"];
    }

    delete() {
        this.addressesService
            .delete(this.address.id)
            .subscribe(
            () => this.router.navigate(["/Addresses"]),
            (error: string) => this.errors = error);
    }
}
