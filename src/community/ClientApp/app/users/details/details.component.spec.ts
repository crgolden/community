import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { DetailsPage } from "../../../test/page-models/users/details-page"
import { DetailsComponent } from "./details.component";
import { User } from "../user"

const userId = "1",
    firstName = "Jim",
    lastName = "Brown",
    user: User = {
        id: userId,
        firstName: firstName,
        lastName: lastName
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

    it("should have the user", () => {
        expect(component.model.id).toBe(userId);
        expect(component.model.firstName).toBe(firstName);
        expect(component.model.lastName).toBe(lastName);
    });

    it("should display user details", () => {
        expect(page.firstName).toBe(firstName);
        expect(page.lastName).toBe(lastName);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(1, "should have 1 routerLink");
        expect(routerLinks[0].linkParams).toBe("/Users");
    });

    it("can click Users link in template", () => {
        const usersLinkDebugElement = routerLinkDebugElements[0],
            usersLink = routerLinks[0];

        expect(usersLink.navigatedTo).toBeNull("should not have navigated yet");

        usersLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(usersLink.navigatedTo).toBe("/Users");
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
                        data: { "user": user }
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
