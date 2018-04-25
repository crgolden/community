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
    event = new Event();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router) {
        this.event.userId = this.eventsService.getUser().id;
    }

    create(valid: boolean) {
        this.submitted = true;

        if (valid) {
            this.isRequesting = true;
            this.eventsService
                .create(this.event)
                .finally(() => this.isRequesting = false)
                .subscribe(
                (id: string) => this.router.navigate([`/Events/Details/${id}`]),
                (error: string) => this.errors = error);
        }
    }
}
