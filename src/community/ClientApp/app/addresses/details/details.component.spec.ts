import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { DetailsPage } from "../../../test/page-models/addresses/details-page"
import { DetailsComponent } from "./details.component";
import { Address } from "../address"

const addressId = "1",
    street = "6700 Burnet Rd",
    street2 = "",
    city = "Austin",
    state = "TX",
    zipCode = "78757",
    address: Address = {
        id: addressId,
        street: street,
        street2: street2,
        city: city,
        state: state,
        zipCode: zipCode
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

    it("should have the address", () => {
        expect(component.model.id).toBe(addressId);
        expect(component.model.street).toBe(street);
        expect(component.model.street2).toBe(street2);
        expect(component.model.city).toBe(city);
        expect(component.model.state).toBe(state);
        expect(component.model.zipCode).toBe(zipCode);
    });

    it("should display address details", () => {
        expect(page.street).toBe(street);
        expect(page.street2).toBe(street2);
        expect(page.city).toBe(city);
        expect(page.state).toBe(state);
        expect(page.zipCode).toBe(zipCode);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(3, "should have 3 routerLinks");
        expect(routerLinks[0].linkParams).toBe(`/Addresses/Edit/${addressId}`);
        expect(routerLinks[1].linkParams).toBe(`/Addresses/Delete/${addressId}`);
        expect(routerLinks[2].linkParams).toBe("/Addresses");
    });

    it("can click Addresses/Edit/:addressId link in template", () => {
        const addressLinkDebugElement = routerLinkDebugElements[0],
            addressLink = routerLinks[0];

        expect(addressLink.navigatedTo).toBeNull("should not have navigated yet");

        addressLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(addressLink.navigatedTo).toBe(`/Addresses/Edit/${addressId}`);
    });

    it("can click Addresses/Delete/:recipeId link in template", () => {
        const addressLinkDebugElement = routerLinkDebugElements[1],
            addressLink = routerLinks[1];

        expect(addressLink.navigatedTo).toBeNull("should not have navigated yet");

        addressLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(addressLink.navigatedTo).toBe(`/Addresses/Delete/${addressId}`);
    });

    it("can click Addresses link in template", () => {
        const addressesLinkDebugElement = routerLinkDebugElements[2],
            addressesLink = routerLinks[2];

        expect(addressesLink.navigatedTo).toBeNull("should not have navigated yet");

        addressesLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(addressesLink.navigatedTo).toBe("/Addresses");
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
                        data: { "address": address }
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
