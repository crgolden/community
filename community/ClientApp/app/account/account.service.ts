import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { User } from "../users/user"

import { IRegister } from "./register/register.interface";
import { ILogin } from "./login/login.interface";

@Injectable()
export class AccountService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    register(value: IRegister): Observable<boolean> {

        const that = this,
            body = JSON.stringify(value),
            options = { headers: that.headers }

        return that.http
            .post<User>("/account/register", body, options)
            .map(res => {
                that.setUser(res);
                return true;
            })
            .catch(that.handleError);
    }

    login(value: ILogin): Observable<boolean> {
        const that = this,
            body = JSON.stringify(value),
            options = { headers: that.headers };

        return that.http
            .post<User>("/account/login", body, options)
            .map(res => {
                that.setUser(res);
                return true;
            })
            .catch(that.handleError);
    }

    logout() {
        const that = this,
            options = { headers: that.headers };

        return that.http
            .post<true>("/account/logout", null, options)
            .map(() => {
                that.removeUser();
            })
            .catch(that.handleError);
    }
}