import { } from "jasmine";

import { ComponentFixture, TestBed, fakeAsync } from "@angular/core/testing";
import { Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { CreatePage } from "../../../test/page-models/events/create-page"
import { CreateComponent } from "./create.component";
import { EventsService } from "../events.service"

let component: CreateComponent,
    fixture: ComponentFixture<CreateComponent>,
    page: CreatePage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[],
    eventsService: EventsService,
    router: Router;

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("CreateComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have a new event", () => {
        expect(component.model.id).toBeUndefined();
        expect(component.model.name).toBeUndefined();
        expect(component.model.details).toBeUndefined();
        expect(component.model.date).toBeUndefined();
        expect(component.model.userId).toBeUndefined();
        expect(component.model.addressId).toBeUndefined();
        expect(component.model.street).toBeUndefined();
        expect(component.model.street2).toBeUndefined();
        expect(component.model.city).toBeUndefined();
        expect(component.model.state).toBeUndefined();
        expect(component.model.zipCode).toBeUndefined();
    });

    it("should display blank inputs", () => {
        fixture.whenStable().then(() => {
            expect(page.name.value).toBe("");
            expect(page.details.value).toBe("");
            expect(page.date.value).toBe("");
            expect(page.street.value).toBe("");
            expect(page.street2.value).toBe("");
            expect(page.city.value).toBe("");
            expect(page.state.value).toBe("");
            expect(page.zipCode.value).toBe("");
        });
    });

    it("should call create and navigate on submit", () => {
        fakeAsync(() => {
            component.create(true);
            expect(eventsService.create).toHaveBeenCalled();
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
            CreateComponent,
            RouterLinkDirectiveStub,
            RouterOutletStubComponent
        ],
        providers: [
            {
                provide: Router,
                useValue: jasmine.createSpyObj("Router", ["navigateByUrl"])
            },
            {
                provide: EventsService,
                useValue: jasmine.createSpyObj("EventsService", ["create"])
            }
        ]
    });
    fixture = TestBed.createComponent(CreateComponent);
    component = fixture.componentInstance;
    eventsService = fixture.debugElement.injector.get(EventsService);
    router = fixture.debugElement.injector.get(Router);
    page = new CreatePage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
