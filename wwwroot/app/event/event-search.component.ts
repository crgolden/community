import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { EventSearchService } from './event-search.service';
import { Event } from './event';
@Component({
    moduleId: module.id,
    selector: 'event-search',
    templateUrl: './event-search.component.html',
    styleUrls: ['./event-search.component.css'],
    providers: [EventSearchService]
})
export class EventSearchComponent implements OnInit {
    events: Observable<Event[]>;
    private searchTerms = new Subject<string>();
    constructor(
        private eventSearchService: EventSearchService,
        private router: Router) { }
    // Push a search term into the observable stream.
    search(term: string): void {
        this.searchTerms.next(term);
    }
    ngOnInit(): void {
        this.events = this.searchTerms
            .debounceTime(300)        // wait for 300ms pause in events
            .distinctUntilChanged()   // ignore if next search term is same as previous
            .switchMap(term => term   // switch to new observable each time
                // return the http search observable
                ? this.eventSearchService.search(term)
                // or the observable of empty events if no search term
                : Observable.of<Event[]>([]))
            .catch(error => {
                // TODO: real error handling
                console.log(error);
                return Observable.of<Event[]>([]);
            });
    }
    gotoDetail(event: Event): void {
        let link = ['/detail', event.id];
        this.router.navigate(link);
    }
}
