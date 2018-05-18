import { } from "jasmine";

import { ComponentFixture, TestBed, fakeAsync } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { DeletePage } from "../../../test/page-models/events/delete-page"
import { DeleteComponent } from "./delete.component";
import { Event } from "../event"
import { EventsService } from "../events.service"

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
let component: DeleteComponent,
    fixture: ComponentFixture<DeleteComponent>,
    page: DeletePage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[],
    eventsService: EventsService,
    router: Router;

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("DeleteComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have the event", () => {
        expect(component.model.id).toBe(eventId);
        expect(component.model.name).toBe(name);
        expect(component.model.details).toBe(details);
    });

    it("should display event details", () => {
        expect(page.name).toBe(name);
        expect(page.details).toBe(details);
        expect(page.date).toBe(dateText);
    });

    it("should call delete and navigate on submit", () => {
        fakeAsync(() => {
            component.delete();
            expect(eventsService.delete).toHaveBeenCalled();
            expect(router.navigateByUrl).toHaveBeenCalled();
        });
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(1, "should have 1 routerLink");
        expect(routerLinks[0].linkParams).toBe("/Events");
    });

    it("can click Events link in template", () => {
        const eventsLinkDebugElement = routerLinkDebugElements[0],
            eventsLink = routerLinks[0];

        expect(eventsLink.navigatedTo).toBeNull("should not have navigated yet");

        eventsLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(eventsLink.navigatedTo).toBe("/Events");
    });

});

function setup() {
    TestBed.configureTestingModule({
        declarations: [
            DeleteComponent,
            RouterLinkDirectiveStub,
            RouterOutletStubComponent
        ],
        providers: [
            {
                provide: ActivatedRoute,
                useValue: {
                    snapshot: {
                        data: { "event": event }
                    }
                }
            },
            {
                provide: Router,
                useValue: jasmine.createSpyObj("Router", ["navigateByUrl"])
            },
            {
                provide: EventsService,
                useValue: jasmine.createSpyObj("EventsService", ["delete"])
            }
        ]
    });
    fixture = TestBed.createComponent(DeleteComponent);
    component = fixture.componentInstance;
    eventsService = fixture.debugElement.injector.get(EventsService);
    router = fixture.debugElement.injector.get(Router);
    page = new DeletePage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
