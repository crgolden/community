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
    isRequesting: boolean = false;
    submitted: boolean = false;
    users: User[];

    constructor(private readonly usersService: UsersService, private readonly router: Router) { }

    ngOnInit(): void {

        var that = this;

        that.usersService
            .getUsers()
            .subscribe(users => {
                    that.users = users;
                },
                error => that.errors = error);
    }

}
