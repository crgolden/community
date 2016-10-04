import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

import { EventService } from '../event/event.service';
import { Event } from '../event/event';

@Component({
    selector: 'my-event-detail',
    templateUrl: './app/event-detail/event-detail.component.html',
    styleUrls: ['./app/event-detail/event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
    @Input()
    event: Event;
    constructor(
        private eventService: EventService,
        private route: ActivatedRoute) {
    }
    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            this.eventService.getEvent(id)
                .then(event => this.event = event);
        });
    }
    goBack(): void {
        window.history.back();
    }
    save(): void {
        this.eventService.update(this.event)
            .then(() => this.goBack());
    }
}
