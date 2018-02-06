import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { User } from "../users/user"

import { IRegister } from "./register/register.interface";
import { ILogin } from "./login/login.interface";

@Injectable()
export class AccountService extends AppService {

    private loggedIn: boolean = false;
    private user: User;

    constructor(private readonly http: HttpClient) {
        super();
        this.user = new User();
    }

    register(value: IRegister): Observable<void> {

        var that = this;
        const body = JSON.stringify(value),
            httpOptions = {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' })
            }

        return that.http
            .post<User>("/account/register", body, httpOptions)
            .map(res => {
                    that.setUser(res);
                    that.loggedIn = true;
            })
            .catch(that.handleError);
    }

    login(value: ILogin): Observable<boolean> {

        var that = this;
        const body = JSON.stringify(value),
            httpOptions = {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' })
            };

        return that.http
            .post<User>("/account/login", body, httpOptions)
            .map(res => {
                    that.setUser(res);
                    that.loggedIn = true;
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