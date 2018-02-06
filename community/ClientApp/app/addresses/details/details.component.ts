import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Component({
    selector: "addresses-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;
    address: Address;

    constructor(
        private readonly addressesService: AddressesService,
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly location: Location) { }

    ngOnInit(): void {

        var that = this;
        const id = this.route.snapshot.paramMap.get("id");
        
        if (typeof id === "string") {
            that.addressesService
                .getAddress(id)
                .subscribe(address => {
                    that.address = address;
                },
                error => that.errors = error);
        }
    }

    goBack(): void {
        this.location.back();
    }
}
