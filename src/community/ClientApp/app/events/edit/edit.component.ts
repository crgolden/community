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
    model = new Event();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.model = this.route.snapshot.data["event"];
    }

    edit(valid: boolean) {
        if (valid) {
            this.eventsService
                .edit(this.model)
                .subscribe(
                    () => this.router.navigate([`/Events/Details/${this.model.id}`]),
                    (error: string) => this.errors = error);
        }
    }
}
