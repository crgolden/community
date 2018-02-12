import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "events-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent {

    errors: string = "";
    events = new Array<Event>();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router) {
    }

    ngOnInit(): void {
        var that = this;

        that.eventsService.index()
            .subscribe(
                (events: Event[] | string) => {
                    if (events instanceof Array) {
                        that.events = events;
                    }
                },
                (error: string) => that.errors = error);
    }
}
