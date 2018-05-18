import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";

import { AppService } from "../app.service";
import { Event } from "./event"

@Injectable()
export class EventsService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    index(): Observable<string | Event[]> {

        return this.http
            .get<Event[]>("/api/v1/Events/Index").pipe(
            catchError(this.handleError));
    }

    details(id: string | null): Observable<string | Event> {

        return this.http
            .get<Event>(`/api/v1/Events/Details/${id}`).pipe(
            catchError(this.handleError));
    }

    create(event: Event): Observable<string | Event> {

        const body = JSON.stringify(event),
            options = { headers: this.getHeaders() };

        return this.http
            .post<Event>("/api/v1/Events/Create", body, options).pipe(
            catchError(this.handleError));
    }

    edit(event: Event) {
        const body = JSON.stringify(event),
            options = { headers: this.getHeaders() };

        return this.http
            .put(`/api/v1/Events/Edit/${event.id}`, body, options).pipe(
            catchError(this.handleError));
    }

    delete(id: string | undefined) {
        const options = { headers: this.getHeaders() };

        return this.http
            .delete(`/api/v1/Events/Delete/${id}`, options).pipe(
            catchError(this.handleError));
    }
}
