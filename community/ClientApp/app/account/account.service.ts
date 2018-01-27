import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { User } from "../models/user"

import { Register } from "./register/register";
import { Login } from "./login/login";

@Injectable()
export class AccountService extends AppService {

    private loggedIn: boolean = false;
    private user: User;

    constructor(private readonly http: Http) {
        super();
        this.user = new User();
    }

    register(value: Register): Observable<boolean> {

        var that = this;
        const headers = new Headers({
                  'Content-Type': "application/json",
                  'Authorization': null
              }),
            body = JSON.stringify(value),
            options = new RequestOptions({ headers: headers });

        return that.http
            .post("/account/register", body, options)
            .map(res => {
                if (res.status === 200) {
                    that.setUser(res.json());
                    that.loggedIn = true;
                    return true;
                } else {
                    return false;
                }
            })
            .catch(that.handleError);
    }

    login(value: Login): Observable<boolean> {

        var that = this;
        const headers = new Headers({
                  'Content-Type': "application/json",
                  'Authorization': null
              }),
            body = JSON.stringify(value);
        
        return that.http
            .post("/account/login", body, { headers })
            .map(res => {
                if (res.status === 200) {
                    that.setUser(res.json());
                    that.loggedIn = true;
                    return true;
                } else {
                    return false;
                }
            })
            .catch(that.handleError);
    }

    logout() {
        this.user = new User();
        this.loggedIn = false;
    }

    isLoggedIn() {
        return this.loggedIn;
    }

    private setUser(user: any) {
        this.user.email = user.email;
        this.user.id = user.id;
        this.user.firstName = user.firstName;
        this.user.lastName = user.lastName;
        this.user.token = user.token;
    }
}