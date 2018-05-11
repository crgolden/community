import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { Address } from "../address"

@Component({
    selector: "addresses-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent implements OnInit {

    addresses: Address[] = [];

    constructor(private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.addresses = this.route.snapshot.data["addresses"];
    }
}
