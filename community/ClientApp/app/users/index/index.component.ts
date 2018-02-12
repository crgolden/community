import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { UsersService } from "../users.service"
import { User } from "../user"

@Component({
    selector: "users-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./index.component.css"]
})
export class IndexComponent {

    errors: string = "";
    users = new Array<User>();

    constructor(
        private readonly usersService: UsersService,
        private readonly router: Router) {
    }

    ngOnInit(): void {
        var that = this;

        that.usersService.index()
            .subscribe(
                (users: User[] | string) => {
                    if (users instanceof Array) {
                        that.users = users;
                    }
                },
                (error: string) => that.errors = error);
    }
}
