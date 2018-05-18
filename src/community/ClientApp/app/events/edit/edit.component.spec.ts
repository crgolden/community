import { } from "jasmine";

import { ComponentFixture, TestBed, fakeAsync } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { EditPage } from "../../../test/page-models/events/edit-page"
import { EditComponent } from "./edit.component";
import { Event } from "../event"
import { EventsService } from "../events.service"

const eventId = "1",
    name = "Drinks",
    details = "Let's have some drinks at the Yard Bar.",
    date = new Date(),
    event: Event = {
        id: eventId,
        name: name,
        details: details,
        date: date
    };
let component: EditComponent,
    fixture: ComponentFixture<EditComponent>,
    page: EditPage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[],
    eventsService: EventsService,
    router: Router;

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("EditComponent", () => {

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
        fixture.whenStable().then(() => {
            expect(page.name.value).toBe(name);
            expect(page.details.value).toBe(details);
            expect(page.date.value).toBe(date.toString());
        });
    });

    it("should call edit and navigate on submit", () => {
        fakeAsync(() => {
            component.edit(true);
            expect(eventsService.edit).toHaveBeenCalled();
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
        imports: [FormsModule],
        declarations: [
            EditComponent,
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
                useValue: jasmine.createSpyObj("EventsService", ["edit"])
            }
        ]
    });
    fixture = TestBed.createComponent(EditComponent);
    component = fixture.componentInstance;
    eventsService = fixture.debugElement.injector.get(EventsService);
    router = fixture.debugElement.injector.get(Router);
    page = new EditPage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
