import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { IndexPage } from "../../../test/page-models/events/index-page"
import { IndexComponent } from "./index.component";
import { Event } from "../event"

const eventId1 = "1",
    eventId2 = "2",
    name1 = "Drinks",
    name2 = "Softball",
    event1: Event = {
        id: eventId1,
        name: name1
    },
    event2: Event = {
        id: eventId2,
        name: name2
    },
    events: Array<Event> = [event1, event2];
let component: IndexComponent,
    fixture: ComponentFixture<IndexComponent>,
    page: IndexPage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[];

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("IndexComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have the events", () => {
        expect(component.events.length).toBe(2);
    });

    it("should display events", () => {
        const eventRow1 = page.rows[1], eventRow2 = page.rows[2];
        let eventRow1Name = eventRow1.children[0].textContent,
            eventRow2Name = eventRow2.children[0].textContent;

        if (eventRow1Name != null) {
            eventRow1Name = eventRow1Name.trim();
        } else {
            eventRow1Name = "";
        }
        if (eventRow2Name != null) {
            eventRow2Name = eventRow2Name.trim();
        } else {
            eventRow2Name = "";
        }

        expect(eventRow1Name).toBe(name1);
        expect(eventRow2Name).toBe(name2);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(3, "should have 3 routerLinks");
        expect(routerLinks[0].linkParams).toBe(`/Events/Details/${eventId1}`);
        expect(routerLinks[1].linkParams).toBe(`/Events/Details/${eventId2}`);
        expect(routerLinks[2].linkParams).toBe("/Events/Create");
    });

    it("can click Events/Details/:events[0].id link in template", () => {
        const event1LinkDebugElement = routerLinkDebugElements[0],
            event1Link = routerLinks[0];

        expect(event1Link.navigatedTo).toBeNull("should not have navigated yet");

        event1LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(event1Link.navigatedTo).toBe(`/Events/Details/${eventId1}`);
    });

    it("can click Events/Details/:events[1].id link in template", () => {
        const event2LinkDebugElement = routerLinkDebugElements[1],
            event2Link = routerLinks[1];

        expect(event2Link.navigatedTo).toBeNull("should not have navigated yet");

        event2LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(event2Link.navigatedTo).toBe(`/Events/Details/${eventId2}`);
    });

    it("can click Events/Create link in template", () => {
        const createLinkDebugElement = routerLinkDebugElements[2],
            createLink = routerLinks[2];

        expect(createLink.navigatedTo).toBeNull("should not have navigated yet");

        createLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(createLink.navigatedTo).toBe("/Events/Create");
    });

});

function setup() {
    TestBed.configureTestingModule({
        declarations: [
            IndexComponent,
            RouterLinkDirectiveStub,
            RouterOutletStubComponent
        ],
        providers: [
            {
                provide: ActivatedRoute,
                useValue: {
                    snapshot: {
                        data: { "events": events }
                    }
                }
            },
            {
                provide: Router,
                useValue: jasmine.createSpyObj("Router", ["navigateByUrl"])
            }
        ]
    });
    fixture = TestBed.createComponent(IndexComponent);
    component = fixture.componentInstance;
    page = new IndexPage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
