import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

import { UsersService } from "../users.service"
import { User } from "../user"

@Component({
    selector: "users-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent {

    errors: string = "";
    isRequesting: boolean = false;
    submitted: boolean = false;
    user: User;

    constructor(
        private readonly usersService: UsersService,
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly location: Location) { }

    ngOnInit(): void {

        var that = this;
        const id = this.route.snapshot.paramMap.get("id");
        
        if (typeof id === "string") {
            that.usersService
                .getUser(id)
                .subscribe(user => {
                    that.user = user;
                },
                error => that.errors = error);
        }
    }

    goBack(): void {
        this.location.back();
    }
}
