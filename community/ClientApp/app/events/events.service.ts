import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { Event } from "./event"

@Injectable()
export class EventsService extends AppService {

    constructor(
        private readonly http: HttpClient) {
        super();
    }

    index(): Observable<Event[] | string> {

        return this.http
            .get<Event[]>("/events/index")
            .catch(this.handleError);
    }

    details(id: string): Observable<Event[] | Event | string> {
        
        return this.http
            .get<Event>(`/Events/Details?id=${id}`)
            .catch(this.handleError);
    }

    create(event: Event): Observable<Event[] | Event | string> {

        const that = this,
            body = JSON.stringify(event),
            options = { headers: that.getHeaders() };
        
        return that.http
            .post<Event>("/Events/Create", body, options)
            .catch(that.handleError);
    }

    edit(event: Event): Observable<Event[] | Event | string> {
        const that = this,
            body = JSON.stringify(event),
            options = { headers: that.getHeaders() };
        
        return that.http
            .post<Event>(`/Events/Edit?id=${event.id}`, body, options)
            .catch(that.handleError);
    }

    delete(id: string): Observable<Event[] | boolean | string> {
        const that = this,
            options = { headers: that.getHeaders() };

        return that.http
            .post<boolean>(`/Events/Delete?id=${id}`, {}, options)
            .catch(that.handleError);
    }

    isEvent(event: Event[] | Event | string): event is Event {
        return event as Event !== undefined;
    }
}