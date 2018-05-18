import { } from "jasmine";

import { ComponentFixture, TestBed, fakeAsync } from "@angular/core/testing";
import { Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { CreatePage } from "../../../test/page-models/addresses/create-page"
import { CreateComponent } from "./create.component";
import { AddressesService } from "../addresses.service"

let component: CreateComponent,
    fixture: ComponentFixture<CreateComponent>,
    page: CreatePage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[],
    addressesService: AddressesService,
    router: Router;

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent { }

describe("CreateComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have a new address", () => {
        expect(component.model.id).toBeUndefined();
        expect(component.model.street).toBeUndefined();
        expect(component.model.street2).toBeUndefined();
        expect(component.model.city).toBeUndefined();
        expect(component.model.state).toBeUndefined();
        expect(component.model.zipCode).toBeUndefined();
        expect(component.model.latitude).toBeUndefined();
        expect(component.model.longitude).toBeUndefined();
        expect(component.model.home).toBeUndefined();
        expect(component.model.userId).toBeUndefined();
        expect(component.model.fullAddress).toBeUndefined();
    });

    it("should display blank inputs", () => {
        fixture.whenStable().then(() => {
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
            expect(addressesService.create).toHaveBeenCalled();
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
                provide: AddressesService,
                useValue: jasmine.createSpyObj("AddressesService", ["create"])
            }
        ]
    });
    fixture = TestBed.createComponent(CreateComponent);
    component = fixture.componentInstance;
    addressesService = fixture.debugElement.injector.get(AddressesService);
    router = fixture.debugElement.injector.get(Router);
    page = new CreatePage(fixture);
    fixture.detectChanges();
    routerLinkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLinkDirectiveStub));
    routerLinks = routerLinkDebugElements.map(de => de.injector.get(RouterLinkDirectiveStub));
}
