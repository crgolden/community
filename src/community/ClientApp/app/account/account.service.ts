import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import { ActivatedRoute } from "@angular/router";

import { AppService } from "../app.service";
import { User } from "../users/user"

import { Register } from "./register/register";
import { Login } from "./login/login";

@Injectable()
export class AccountService extends AppService {

    private returnUrl: string = "";

    constructor(
        private readonly http: HttpClient,
        private readonly route: ActivatedRoute) {
        super();
        this.setReturnUrlFromQueryParams();
    }

    register(value: Register): Observable<string | boolean> {

        const body = JSON.stringify(value),
            options = { headers: this.getHeaders() }

        return this.http
            .post<User>("/api/v1/Account/Register", body, options)
            .map(res => {
                this.setUser(res);
                this.setExpiration(new Date());
                this.isLoggedIn.emit(true);
                return true;
            })
            .catch(this.handleError);
    }

    login(value: Login): Observable<string | boolean> {
        const body = JSON.stringify(value),
            options = { headers: this.getHeaders() };

        return this.http
            .post<User>("/api/v1/Account/Login", body, options)
            .map(res => {
                this.setUser(res);
                this.setExpiration(new Date());
                this.isLoggedIn.emit(true);
                return true;
            })
            .catch(this.handleError);
    }

    logout(): Observable<string | boolean> {
        const options = { headers: this.getHeaders() };

        return this.http
            .post<boolean>("/api/v1/Account/Logout", {}, options)
            .map(() => {
                this.removeUser();
                this.removeExpiration();
                this.isLoggedIn.emit(false);
                return true;
            })
            .catch(this.handleError);
    }

    private setReturnUrlFromQueryParams(): void {
        this.route.queryParams.subscribe(params => {
            var returnUrl = params["returnUrl"];
            if (typeof returnUrl == "undefined") {
                returnUrl = params["ReturnUrl"];
            }
            if (typeof returnUrl == "string") {
                this.returnUrl = returnUrl;
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