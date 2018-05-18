import { } from "jasmine";

import { ComponentFixture, TestBed, fakeAsync } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { EditPage } from "../../../test/page-models/addresses/edit-page"
import { EditComponent } from "./edit.component";
import { Address } from "../address"
import { AddressesService } from "../addresses.service"

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
let component: EditComponent,
    fixture: ComponentFixture<EditComponent>,
    page: EditPage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[],
    addressesService: AddressesService,
    router: Router;

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("EditComponent", () => {

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
        fixture.whenStable().then(() => {
            expect(page.street.value).toBe(street);
            expect(page.street2.value).toBe(street2);
            expect(page.city.value).toBe(city);
            expect(page.state.value).toBe(state);
            expect(page.zipCode.value).toBe(zipCode);
        });
    });

    it("should call edit and navigate on submit", () => {
        fakeAsync(() => {
            component.edit(true);
            expect(addressesService.edit).toHaveBeenCalled();
            expect(router.navigateByUrl).toHaveBeenCalled();
        });
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(1, "should have 1 routerLink");
        expect(routerLinks[0].linkParams).toBe("/Addresses");
    });

    it("can click Addresses link in template", () => {
        const addressesLinkDebugElement = routerLinkDebugElements[0],
            addressesLink = routerLinks[0];

        expect(addressesLink.navigatedTo).toBeNull("should not have navigated yet");

        addressesLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(addressesLink.navigatedTo).toBe("/Addresses");
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
                        data: { "address": address }
                    }
                }
            },
            {
                provide: Router,
                useValue: jasmine.createSpyObj("Router", ["navigateByUrl"])
            },
            {
                provide: AddressesService,
                useValue: jasmine.createSpyObj("AddressesService", ["edit"])
            }
        ]
    });
    fixture = TestBed.createComponent(EditComponent);
    component = fixture.componentInstance;
    addressesService = fixture.debugElement.injector.get(AddressesService);
    router = fixture.debugElement.injector.get(Router);
    page = new EditPage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
