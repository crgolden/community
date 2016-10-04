import { Injectable }    from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Event } from './event';

@Injectable()
export class EventService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private eventsUrl = 'api/events';

    constructor(private http: Http) { }

    getEvents(): Promise<Event[]> {
        return this.http.get(this.eventsUrl)
            .toPromise()
            .then(response => response.json() as Event[])
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

    getEvent(id: number): Promise<Event> {
        return this.getEvents()
            .then(events => events.find(event => event.id === id));
    }

    update(event: Event): Promise<Event> {
        const url = `${this.eventsUrl}/${event.id}`;
        return this.http
            .put(url, JSON.stringify(event), { headers: this.headers })
            .toPromise()
            .then(() => event)
            .catch(this.handleError);
    }
    create(title: string): Promise<Event> {
        return this.http
            .post(this.eventsUrl, JSON.stringify({ title: title }), { headers: this.headers })
            .toPromise()
            .then(res => res.json())
            .catch(this.handleError);
    }
    delete(id: number): Promise<void> {
        const url = `${this.eventsUrl}/${id}`;
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }
}
