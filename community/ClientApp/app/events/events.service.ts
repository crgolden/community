import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { Event } from "./event"

@Injectable()
export class EventsService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    index(): Observable<string | Event[]> {

        return this.http
            .get<Event[]>("/Events/Index")
            .catch(this.handleError);
    }

    details(id: string | null): Observable<string | Event> {
        
        return this.http
            .get<Event>(`/Events/Details?id=${id}`)
            .catch(this.handleError);
    }

    create(event: Event): Observable<string> {

        const body = JSON.stringify(event),
            options = { headers: this.getHeaders() };
        
        return this.http
            .post<string>("/Events/Create", body, options)
            .catch(this.handleError);
    }

    edit(event: Event) {
        const body = JSON.stringify(event),
            options = { headers: this.getHeaders() };
        
        return this.http
            .put(`/Events/Edit?id=${event.id}`, body, options)
            .catch(this.handleError);
    }

    delete(id: string | undefined) {
        const options = { headers: this.getHeaders() };

        return this.http
            .delete(`/Events/Delete?id=${id}`, options)
            .catch(this.handleError);
    }
}
