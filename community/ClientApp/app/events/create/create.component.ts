import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "events-create",
    templateUrl: "./create.component.html",
    styleUrls: ["./create.component.css"]
})
export class CreateComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router) {
    }

    create({ value, valid }: { value: Event, valid: boolean }) {
        var that = this;
        that.submitted = true;

        if (valid) {
            that.isRequesting = true;
            value.userId = that.eventsService.getUser().id;
            that.eventsService.create(value)
                .finally(
                    () => that.isRequesting = false)
                .subscribe(
                    (event: Event[] | Event | string) => {
                        if (that.eventsService.isEvent(event)) {
                            that.router.navigate([`/Events/Details/${event.id}`]);
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
