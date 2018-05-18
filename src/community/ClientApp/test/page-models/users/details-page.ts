import { ComponentFixture } from "@angular/core/testing";

import { DetailsComponent } from "../../../app/users/details/details.component"
import { QueryHelpers } from "../../query-helpers"

export class DetailsPage {
    constructor(fixture: ComponentFixture<DetailsComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<DetailsComponent>;

    get elements() {
        return QueryHelpers.queryAll<HTMLElement>(this.fixture, "dd");
    }
    get firstName() {
        let firstName = this.elements[0].textContent;
        if (typeof firstName === "string") {
            firstName = firstName.trim();
        }
        return firstName;
    }
    get lastName() {
        let lastName = this.elements[1].textContent;
        if (typeof lastName === "string") {
            lastName = lastName.trim();
        }
        return lastName;
    }
}
