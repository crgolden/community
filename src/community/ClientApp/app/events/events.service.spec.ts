import { } from "jasmine";
import { defer as observableDefer } from "rxjs";

import { EventsService } from "./events.service";
import { Event } from "./event"

let httpClientSpy: { get: jasmine.Spy, post: jasmine.Spy, put: jasmine.Spy, delete: jasmine.Spy };
let eventsService: EventsService;
let event1: Event, event2: Event;

describe("EventsService", () => {

    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj("HttpClient", ["get", "post", "put", "delete"]);
        eventsService = new EventsService(httpClientSpy as any);
        event1 = { id: "1" };
        event2 = { id: "2" };
    });

    it("index should return a list of events", () => {
        const events: Array<Event> = [event1, event2];

        httpClientSpy.get.and.returnValue(observableDefer(() => Promise.resolve(events)));

        eventsService
            .index()
            .subscribe(
                res => expect(res).toEqual(events, "expected events"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

    it("details should return an event", () => {
        httpClientSpy.get.and.returnValue(observableDefer(() => Promise.resolve(event1)));

        eventsService
            .details("1")
            .subscribe(
                res => expect(res).toEqual(event1, "expected event1"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

    it("create should return an event", () => {
        httpClientSpy.post.and.returnValue(observableDefer(() => Promise.resolve(event1)));

        eventsService
            .create(event1)
            .subscribe(
                res => expect(res).toEqual(event1, "expected event1"),
                fail);

        expect(httpClientSpy.post.calls.count()).toBe(1, "one call");
    });

    it("edit should not return anything", () => {
        httpClientSpy.put.and.returnValue(observableDefer(() => Promise.resolve()));

        eventsService
            .edit(event1)
            .subscribe(
                res => expect(res).toBeUndefined("expected undefined"),
                fail);

        expect(httpClientSpy.put.calls.count()).toBe(1, "one call");
    });

    it("delete should not return anything", () => {
        httpClientSpy.delete.and.returnValue(observableDefer(() => Promise.resolve()));

        eventsService
            .delete(event1.id)
            .subscribe(
                res => expect(res).toBeUndefined("expected undefined"),
                fail);

        expect(httpClientSpy.delete.calls.count()).toBe(1, "one call");
    });

});
