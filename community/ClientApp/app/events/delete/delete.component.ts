import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Component({
    selector: "events-delete",
    templateUrl: "./delete.component.html",
    styleUrls: ["./delete.component.css"]
})
export class DeleteComponent {

    errors: string = "";
    event = new Event();

    constructor(
        private readonly eventsService: EventsService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        var that = this;
        const id = this.route.snapshot.paramMap.get("id");

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

    delete() {
        var that = this;

        if (typeof that.event.id == "string" && that.event.id.length === 36) {
            that.eventsService.delete(that.event.id)
                .subscribe(
                    res => {
                        if (typeof res == "boolean" && res) {
                            that.router.navigate(["/Events"]);
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
