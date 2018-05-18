import { ComponentFixture } from "@angular/core/testing";

import { DeleteComponent } from "../../../app/events/delete/delete.component"
import { QueryHelpers } from "../../query-helpers"

export class DeletePage {
    constructor(fixture: ComponentFixture<DeleteComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<DeleteComponent>;

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
