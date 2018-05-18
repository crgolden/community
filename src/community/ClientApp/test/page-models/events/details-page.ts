import { ComponentFixture } from "@angular/core/testing";

import { DetailsComponent } from "../../../app/events/details/details.component"
import { QueryHelpers } from "../../query-helpers"

export class DetailsPage {
    constructor(fixture: ComponentFixture<DetailsComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<DetailsComponent>;

    get elements() {
        return QueryHelpers.queryAll<HTMLElement>(this.fixture, "dd");
    }
    get name() {
        let name = this.elements[0].textContent;
        if (typeof name === "string") {
            name = name.trim();
        }
        return name;
    }
    get details() {
        let details = this.elements[1].textContent;
        if (typeof details === "string") {
            details = details.trim();
        }
        return details;
    }
    get date() {
        let date = this.elements[2].textContent;
        if (typeof date === "string") {
            date = date.trim();
        }
        return date;
    }
}
