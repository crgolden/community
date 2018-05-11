import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { Event } from "../event"

@Component({
    selector: "events-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent implements OnInit {

    events: Event[] = [];

    constructor(private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.events = this.route.snapshot.data["events"];
    }
}
