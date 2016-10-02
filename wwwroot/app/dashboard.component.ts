import { Component, OnInit } from '@angular/core';
import { Event } from './event';
import { EventService } from './event.service';
import { Router } from '@angular/router';

@Component({
    selector: 'my-dashboard',
    templateUrl: 'app/dashboard.component.html',
    styleUrls: ['app/dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    events: Event[] = [];
    slicedEvents: Event[] = [];

    constructor(
        private router: Router,
        private eventService: EventService) {
    }
    ngOnInit(): void {
        this.eventService.getEvents()
            .then(events => this.events = events)
            .then(events => this.slicedEvents = events.slice(1, 5));
    }

    gotoDetail(event: Event): void {
        let link = ['/detail', event.id];
        this.router.navigate(link);
    }
}