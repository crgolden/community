import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import { ActivatedRoute } from "@angular/router";

import { AppService } from "../app.service";
import { User } from "../users/user"

import { IRegister } from "./register/register.interface";
import { ILogin } from "./login/login.interface";

@Injectable()
export class AccountService extends AppService {
    
    private returnUrl: string = "";

    constructor(
        private readonly http: HttpClient,
        private readonly route: ActivatedRoute) {
        super();
        this.setReturnUrlFromQueryParams();
    }

    register(value: IRegister): Observable<string | boolean> {

        const that = this,
            body = JSON.stringify(value),
            options = { headers: that.getHeaders() }

        return that.http
            .post<User>("/Account/Register", body, options)
            .map(res => {
                that.setUser(res);
                that.setExpiration(new Date());
                that.isLoggedIn.emit(true);
                return true;
            })
            .catch(that.handleError);
    }

    login(value: ILogin): Observable<string | boolean> {
        const that = this,
            body = JSON.stringify(value),
            options = { headers: that.getHeaders() };

        return that.http
            .post<User>("/Account/Login", body, options)
            .map(res => {
                that.setUser(res);
                that.setExpiration(new Date());
                that.isLoggedIn.emit(true);
                return true;
            })
            .catch(that.handleError);
    }

    logout(): Observable<string | boolean> {
        const that = this,
            options = { headers: that.getHeaders() };

        return that.http
            .post<boolean>("/Account/Logout", {}, options)
            .map(() => {
                that.removeUser();
                that.removeExpiration();
                that.isLoggedIn.emit(false);
                return true;
            })
            .catch(that.handleError);
    }

    private setReturnUrlFromQueryParams(): void {
        var that = this;

        that.route.queryParams.subscribe(params => {
            var returnUrl = params["returnUrl"];
            if (typeof returnUrl == "undefined") {
                returnUrl = params["ReturnUrl"];
            }
            if (typeof returnUrl == "string") {
                that.returnUrl = returnUrl;
            }
        });
    }

    setReturnUrl(returnUrl: string) {
        this.returnUrl = returnUrl;
    }

    getReturnUrl(): string {
        return this.returnUrl;
    }
}