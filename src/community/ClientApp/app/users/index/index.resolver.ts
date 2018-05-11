import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot } from "@angular/router";

import { UsersService } from "../users.service"
import { User } from "../user"

@Injectable()
export class IndexResolver implements Resolve<string | User[]> {

    constructor(private readonly usersService: UsersService) {
    }

    resolve(route: ActivatedRouteSnapshot) {
        return this.usersService.index();
    }
}