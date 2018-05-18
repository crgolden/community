import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { DetailsPage } from "../../../test/page-models/events/details-page"
import { DetailsComponent } from "./details.component";
import { Event } from "../event"

const options = { year: "numeric", month: "short", day: "numeric" },
    eventId = "1",
    name = "Drinks",
    details = "Let's have some drinks at the Yard Bar.",
    date = new Date(),
    dateText = date.toLocaleDateString("en-US", options),
    event: Event = {
        id: eventId,
        name: name,
        details: details,
        date: date
    };
let component: DetailsComponent,
    fixture: ComponentFixture<DetailsComponent>,
    page: DetailsPage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[];

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("DetailsComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have the event", () => {
        expect(component.model.id).toBe(eventId);
        expect(component.model.name).toBe(name);
        expect(component.model.details).toBe(details);
        expect(component.model.date).toBe(date);
    });

    it("should display event details", () => {
        expect(page.name).toBe(name);
        expect(page.details).toBe(details);
        expect(page.date).toBe(dateText);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(3, "should have 3 routerLinks");
        expect(routerLinks[0].linkParams).toBe(`/Events/Edit/${eventId}`);
        expect(routerLinks[1].linkParams).toBe(`/Events/Delete/${eventId}`);
        expect(routerLinks[2].linkParams).toBe("/Events");
    });

    it("can click Events/Edit/:eventId link in template", () => {
        const eventLinkDebugElement = routerLinkDebugElements[0],
            eventLink = routerLinks[0];

        expect(eventLink.navigatedTo).toBeNull("should not have navigated yet");

        eventLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(eventLink.navigatedTo).toBe(`/Events/Edit/${eventId}`);
    });

    it("can click Events/Delete/:eventId link in template", () => {
        const eventLinkDebugElement = routerLinkDebugElements[1],
            eventLink = routerLinks[1];

        expect(eventLink.navigatedTo).toBeNull("should not have navigated yet");

        eventLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(eventLink.navigatedTo).toBe(`/Events/Delete/${eventId}`);
    });

    it("can click Events link in template", () => {
        const eventsLinkDebugElement = routerLinkDebugElements[2],
            eventsLink = routerLinks[2];

        expect(eventsLink.navigatedTo).toBeNull("should not have navigated yet");

        eventsLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(eventsLink.navigatedTo).toBe("/Events");
    });

});

function setup() {
    TestBed.configureTestingModule({
        declarations: [
            DetailsComponent,
            RouterLinkDirectiveStub,
            RouterOutletStubComponent
        ],
        providers: [
            {
                provide: ActivatedRoute,
                useValue: {
                    snapshot: {
                        data: { "event": event}
                    }
                }
            },
            {
                provide: Router,
                useValue: jasmine.createSpyObj("Router", ["navigateByUrl"])
            }
        ]
    });
    fixture = TestBed.createComponent(DetailsComponent);
    component = fixture.componentInstance;
    page = new DetailsPage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
