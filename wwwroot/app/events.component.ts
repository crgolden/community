import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Event } from './event';
import { EventService } from './event.service';

@Component({
    selector: 'my-events',
    templateUrl: 'app/events.component.html',
    styleUrls: ['app/events.component.css']
})
export class EventsComponent implements OnInit {
    events: Event[];
    selectedEvent: Event;

    constructor(
        private router: Router,
        private eventService: EventService) { }

    getEvents(): void {
        this.eventService.getEvents().then(events => this.events = events);
    }

    ngOnInit(): void {
        this.getEvents();
    }

    onSelect(event: Event): void {
        this.selectedEvent = event;
    }

    gotoDetail(): void {
        this.router.navigate(['/detail', this.selectedEvent.id]);
    }
    add(title: string): void {
        title = title.trim();
        if (!title) { return; }
        this.eventService.create(title)
            .then(event => {
                this.events.push(event);
                this.selectedEvent = null;
            });
    }
    delete(event: Event): void {
        this.eventService
            .delete(event.id)
            .then(() => {
                this.events = this.events.filter(h => h !== event);
                if (this.selectedEvent === event) { this.selectedEvent = null; }
            });
    }
}
