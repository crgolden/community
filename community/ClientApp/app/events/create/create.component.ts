import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

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
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly location: Location) { }

    saveEvent({ value, valid }: { value: Event, valid: boolean }) {

        var that = this;

        if (valid) {
            that.submitted = true;
            that.isRequesting = true;
            value.userId = that.eventsService.user.id;
            that.eventsService
                .createEvent(value)
                .finally(() => that.isRequesting = false)
                .subscribe(res => {
                    if (typeof res.id !== "undefined" && res.id.length === 36) {
                        that.router.navigate([`/events/details/${res.id}`]);
                    }
                },
                error => that.errors = error);
        }
    }

    goBack(): void {
        this.location.back();
    }
}
