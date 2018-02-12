import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { UsersService } from "../users.service"
import { User } from "../user"

@Component({
    selector: "users-details",
    templateUrl: "./details.component.html",
    styleUrls: ["./details.component.css"]
})
export class DetailsComponent {

    errors: string = "";
    user = new User();

    constructor(
        private readonly usersService: UsersService,
        private readonly router: Router,
        private readonly route: ActivatedRoute) {
    }

    ngOnInit(): void {
        var that = this;
        const id = that.route.snapshot.paramMap.get("id");

        if (typeof id == "string" && id.length === 36) {
            that.usersService.details(id)
                .subscribe(
                    (user: User[] | User | string) => {
                        if (that.usersService.isUser(user)) {
                            that.user.id = user.id;
                            that.user.firstName = user.firstName;
                            that.user.lastName = user.lastName;
                        }
                    },
                    (error: string) => that.errors = error);
        }
    }
}
