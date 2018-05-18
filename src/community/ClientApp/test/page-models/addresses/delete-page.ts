import { ComponentFixture } from "@angular/core/testing";

import { DeleteComponent } from "../../../app/addresses/delete/delete.component"
import { QueryHelpers } from "../../query-helpers"

export class DeletePage {
    constructor(fixture: ComponentFixture<DeleteComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<DeleteComponent>;

    get elemens() {
        return QueryHelpers.queryAll<HTMLElement>(this.fixture, "dd");
    }
    get street() {
        let title = this.elemens[0].textContent;
        if (typeof title === "string") {
            title = title.trim();
        }
        return title;
    }
    get street2() {
        let description = this.elemens[1].textContent;
        if (typeof description === "string") {
            description = description.trim();
        }
        return description;
    }
    get city() {
        let city = this.elemens[2].textContent;
        if (typeof city === "string") {
            city = city.trim();
        }
        return city;
    }
    get state() {
        let state = this.elemens[3].textContent;
        if (typeof state === "string") {
            state = state.trim();
        }
        return state;
    }
    get zipCode() {
        let zipCode = this.elemens[4].textContent;
        if (typeof zipCode === "string") {
            zipCode = zipCode.trim();
        }
        return zipCode;
    }
}
