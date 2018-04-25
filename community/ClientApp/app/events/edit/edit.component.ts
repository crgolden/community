import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "event-edit",
    templateUrl: "./edit.component.html",
    styleUrls: ["./edit.component.css"]
})
export class EditComponent implements OnInit {

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
        this.event = this.route.snapshot.data["event"];
    }

    edit(valid: boolean) {
        this.submitted = true;

        if (valid) {
            this.isRequesting = true;
            this.eventsService
                .edit(this.event)
                .finally(() => this.isRequesting = false)
                .subscribe(
                () => this.router.navigate([`/Events/Details/${this.event.id}`]),
                (error: string) => this.errors = error);
        }
    }
}
