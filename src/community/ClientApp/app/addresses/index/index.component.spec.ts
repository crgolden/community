import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { IndexPage } from "../../../test/page-models/addresses/index-page"
import { IndexComponent } from "./index.component";
import { Address } from "../address"

const addressId1 = "1",
    addressId2 = "2",
    fullAddress1 = "6700 Burnet Rd, Austin, TX 78757",
    fullAddress2 = "6701 Lakewood Dr, Austin, TX 78731",
    address1: Address = {
        id: addressId1,
        fullAddress: fullAddress1
    },
    address2: Address = {
        id: addressId2,
        fullAddress: fullAddress2
    },
    addresses: Array<Address> = [address1, address2];
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

    it("should have the addresses", () => {
        expect(component.addresses.length).toBe(2);
    });

    it("should display addresses", () => {
        const addressRow1 = page.rows[1], addressRow2 = page.rows[2];
        let addressRow1FullAddress = addressRow1.children[0].textContent,
            addressRow2FullAddress = addressRow2.children[0].textContent;

        if (addressRow1FullAddress != null) {
            addressRow1FullAddress = addressRow1FullAddress.trim();
        } else {
            addressRow1FullAddress = "";
        }
        if (addressRow2FullAddress != null) {
            addressRow2FullAddress = addressRow2FullAddress.trim();
        } else {
            addressRow2FullAddress = "";
        }

        expect(addressRow1FullAddress).toBe(fullAddress1);
        expect(addressRow2FullAddress).toBe(fullAddress2);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(3, "should have 3 routerLinks");
        expect(routerLinks[0].linkParams).toBe(`/Addresses/Details/${addressId1}`);
        expect(routerLinks[1].linkParams).toBe(`/Addresses/Details/${addressId2}`);
        expect(routerLinks[2].linkParams).toBe("/Addresses/Create");
    });

    it("can click Addresses/Details/:addresses[0].id link in template", () => {
        const address1LinkDebugElement = routerLinkDebugElements[0],
            address1Link = routerLinks[0];

        expect(address1Link.navigatedTo).toBeNull("should not have navigated yet");

        address1LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(address1Link.navigatedTo).toBe(`/Addresses/Details/${addressId1}`);
    });

    it("can click Addresses/Details/:addresses[1].id link in template", () => {
        const address2LinkDebugElement = routerLinkDebugElements[1],
            address2Link = routerLinks[1];

        expect(address2Link.navigatedTo).toBeNull("should not have navigated yet");

        address2LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(address2Link.navigatedTo).toBe(`/Addresses/Details/${addressId2}`);
    });

    it("can click Addresses/Create link in template", () => {
        const createLinkDebugElement = routerLinkDebugElements[2],
            createLink = routerLinks[2];

        expect(createLink.navigatedTo).toBeNull("should not have navigated yet");

        createLinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(createLink.navigatedTo).toBe("/Addresses/Create");
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
                        data: { "addresses": addresses }
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
