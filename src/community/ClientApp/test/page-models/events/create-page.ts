import { ComponentFixture } from "@angular/core/testing";

import { CreateComponent } from "../../../app/events/create/create.component"
import { QueryHelpers } from "../../query-helpers"

export class CreatePage {
    constructor(fixture: ComponentFixture<CreateComponent>) {
        this.fixture = fixture;
    }

    fixture: ComponentFixture<CreateComponent>;

    get inputs() {
        return QueryHelpers.queryAll<HTMLInputElement>(this.fixture, "input");
    }
    get name() {
        return this.inputs[0];
    }
    get details() {
        return this.inputs[1];
    }
    get date() {
        return this.inputs[2];
    }
    get street() {
        return this.inputs[3];
    }
    get street2() {
        return this.inputs[4];
    }
    get city() {
        return this.inputs[5];
    }
    get state() {
        return this.inputs[6];
    }
    get zipCode() {
        return this.inputs[7];
    }
}
