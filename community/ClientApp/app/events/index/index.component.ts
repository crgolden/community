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
    isRequesting: boolean = false;
    submitted: boolean = false;
    events: Event[];

    constructor(private readonly eventsService: EventsService, private readonly router: Router) { }

    ngOnInit(): void {

        var that = this;
        
        that.eventsService
            .getEvents()
            .subscribe(events => {
                    that.events = events;
                },
                error => that.errors = error);
    }

}
