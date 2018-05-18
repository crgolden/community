import { ComponentFixture } from "@angular/core/testing";

import { DetailsComponent } from "../../../app/addresses/details/details.component"
import { QueryHelpers } from "../../query-helpers"

export class DetailsPage {
    constructor(fixture: ComponentFixture<DetailsComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<DetailsComponent>;

    get elements() {
        return QueryHelpers.queryAll<HTMLElement>(this.fixture, "dd");
    }
    get street() {
        let street = this.elements[0].textContent;
        if (typeof street === "string") {
            street = street.trim();
        }
        return street;
    }
    get street2() {
        let street2 = this.elements[1].textContent;
        if (typeof street2 === "string") {
            street2 = street2.trim();
        }
        return street2;
    }
    get city() {
        let city = this.elements[2].textContent;
        if (typeof city === "string") {
            city = city.trim();
        }
        return city;
    }
    get state() {
        let state = this.elements[3].textContent;
        if (typeof state === "string") {
            state = state.trim();
        }
        return state;
    }
    get zipCode() {
        let zipCode = this.elements[4].textContent;
        if (typeof zipCode === "string") {
            zipCode = zipCode.trim();
        }
        return zipCode;
    }
}
