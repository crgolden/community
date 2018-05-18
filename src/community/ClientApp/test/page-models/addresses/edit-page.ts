import { ComponentFixture } from "@angular/core/testing";

import { EditComponent } from "../../../app/addresses/edit/edit.component"
import { QueryHelpers } from "../../query-helpers"

export class EditPage {
    constructor(fixture: ComponentFixture<EditComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<EditComponent>;

    get inputs() {
        return QueryHelpers.queryAll<HTMLInputElement>(this.fixture, "input");
    }
    get street() {
        return this.inputs[0];
    }
    get street2() {
        return this.inputs[1];
    }
    get city() {
        return this.inputs[2];
    }
    get state() {
        return this.inputs[3];
    }
    get zipCode() {
        return this.inputs[4];
    }
}
