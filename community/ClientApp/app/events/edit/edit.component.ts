import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "event-edit",
    templateUrl: "./edit.component.html",
    styleUrls: ["./edit.component.css"]
})
export class EditComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;
    event = new Event();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        var that = this;
        const id = that.route.snapshot.paramMap.get("id");

        if (typeof id == "string" && id.length === 36) {
            that.eventsService.details(id)
                .subscribe(
                    (event: Event[] | Event | string) => {
                        if (that.eventsService.isEvent(event)) {
                            that.event.id = event.id;
                            that.event.name = event.name;
                            that.event.details = event.details;
                            that.event.date = event.date;
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }

    edit({ value, valid }: { value: Event, valid: boolean }) {
        var that = this;
        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            that.event.name = value.name;
            that.event.details = value.details;
            that.event.date = value.date;
            that.eventsService.edit(that.event)
                .finally(
                    () => that.isRequesting = false)
                .subscribe(
                    event => {
                        if (event instanceof Event) {
                            that.router.navigate([`/Events/Details/${event.id}`]);
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
