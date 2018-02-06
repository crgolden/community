import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "events-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;
    event: Event;

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly location: Location) { }

    ngOnInit(): void {

        var that = this;
        const id = this.route.snapshot.paramMap.get("id");
        
        if (typeof id === "string") {
            that.eventsService
                .getEvent(id)
                .subscribe(event => {
                    that.event = event;
                },
                error => that.errors = error);
        }
    }

    goBack(): void {
        this.location.back();
    }
}
