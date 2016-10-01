import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { Event } from './event';
@Injectable()
export class EventSearchService {
    constructor(private http: Http) { }
    search(term: string): Observable<Event[]> {
        return this.http
            .get(`events?searchString=${term}`)
            .map((r: Response) => r.json() as Event[]);
    }
}