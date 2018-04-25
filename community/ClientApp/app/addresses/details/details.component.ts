import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { Address } from "../address"

@Component({
    selector: "addresses-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent implements OnInit {

    address = new Address();

    constructor(private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.address = this.route.snapshot.data["address"];
    }
}
