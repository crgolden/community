import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { User } from "../user"

@Component({
    selector: "users-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent implements OnInit {

    users: User[] = [];

    constructor(private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.users = this.route.snapshot.data["users"];
    }
}
