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
    model = new Event();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router) {
    }

    create(valid: boolean) {
        if (valid) {
            this.model.userId = this.eventsService.getUser().id;
            this.eventsService
                .create(this.model)
                .subscribe(
                    (event: string | Event) => {
                        if (typeof event !== "string") {
                            this.router.navigate([`/Events/Details/${event.id}`]);
                        }
                    },
                    (error: string) => this.errors = error);
        }
    }
}
