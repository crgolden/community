import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { User } from "../user"

@Component({
    selector: "users-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent implements OnInit {

    user = new User();

    constructor(private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.user = this.route.snapshot.data["user"];
    }
}
