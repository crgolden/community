import { } from "jasmine";

import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { Component, DebugElement } from "@angular/core";
import { By } from "@angular/platform-browser";

import { RouterLinkDirectiveStub } from "../../../test/router-link-directive-stub";
import { IndexPage } from "../../../test/page-models/users/index-page"
import { IndexComponent } from "./index.component";
import { User } from "../user"

const userId1 = "1",
    userId2 = "2",
    firstName1 = "Jim",
    lastName1 = "Brown",
    firstName2 = "Marlene",
    lastName2 = "Stephens",
    user1: User = {
        id: userId1,
        firstName: firstName1,
        lastName: lastName1
    },
    user2: User = {
        id: userId2,
        firstName: firstName2,
        lastName: lastName2
    },
    users: Array<User> = [user1, user2];
let component: IndexComponent,
    fixture: ComponentFixture<IndexComponent>,
    page: IndexPage,
    routerLinks: RouterLinkDirectiveStub[],
    routerLinkDebugElements: DebugElement[];

@Component({ selector: "router-outlet", template: "" })
class RouterOutletStubComponent {
}

describe("IndexComponent", () => {

    beforeEach(() => {
        setup();
    });

    it("should have the users", () => {
        expect(component.users.length).toBe(2);
    });

    it("should display users", () => {
        const userRow1 = page.rows[1], userRow2 = page.rows[2];
        let userRow1FullName = userRow1.children[0].textContent,
            userRow2FullName = userRow2.children[0].textContent;

        if (userRow1FullName != null) {
            userRow1FullName = userRow1FullName.trim();
        } else {
            userRow1FullName = "";
        }
        if (userRow2FullName != null) {
            userRow2FullName = userRow2FullName.trim();
        } else {
            userRow2FullName = "";
        }

        expect(userRow1FullName).toBe(`${firstName1} ${lastName1}`);
        expect(userRow2FullName).toBe(`${firstName2} ${lastName2}`);
    });

    it("can get RouterLinks from template", () => {
        expect(routerLinks.length).toBe(2, "should have 2 routerLinks");
        expect(routerLinks[0].linkParams).toBe(`/Users/Details/${userId1}`);
        expect(routerLinks[1].linkParams).toBe(`/Users/Details/${userId2}`);
    });

    it("can click Users/Details/:users[0].id link in template", () => {
        const user1LinkDebugElement = routerLinkDebugElements[0],
            user1Link = routerLinks[0];

        expect(user1Link.navigatedTo).toBeNull("should not have navigated yet");

        user1LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(user1Link.navigatedTo).toBe(`/Users/Details/${userId1}`);
    });

    it("can click Users/Details/:users[1].id link in template", () => {
        const user2LinkDebugElement = routerLinkDebugElements[1],
            user2Link = routerLinks[1];

        expect(user2Link.navigatedTo).toBeNull("should not have navigated yet");

        user2LinkDebugElement.triggerEventHandler("click", null);
        fixture.detectChanges();

        expect(user2Link.navigatedTo).toBe(`/Users/Details/${userId2}`);
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
                        data: { "users": users }
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
