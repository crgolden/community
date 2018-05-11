import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "events-delete",
    templateUrl: "./delete.component.html",
    styleUrls: ["./delete.component.css"]
})
export class DeleteComponent implements OnInit {

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

    delete() {
        this.eventsService
            .delete(this.model.id)
            .subscribe(
                () => this.router.navigate(["/Events"]),
                (error: string) => this.errors = error);
    }
}
