import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot } from "@angular/router";

import { EventsService } from "../events.service"
import { Event } from "../event"

@Injectable()
export class DetailsResolver implements Resolve<string | Event> {

    constructor(private readonly eventsService: EventsService) {
    }

    resolve(route: ActivatedRouteSnapshot) {
        return this.eventsService.details(route.paramMap.get("id"));
    }
}