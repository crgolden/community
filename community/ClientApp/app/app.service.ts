import { Injectable, Output, EventEmitter } from "@angular/core"
import { HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { User } from "./users/user"

@Injectable()
export class AppService {

    @Output() isLoggedIn = new EventEmitter<boolean>();

    protected handleError(error: any): Observable<string> {
        const applicationError = error.headers.get("Application-Error");

        if (applicationError) {
            return Observable.throw(applicationError);
        }

        let modelStateErrors: string = "";

        if (!error.type) {
            for (let key in error) {
                if (error.hasOwnProperty(key) && error[key])
                    modelStateErrors += error[key] + "\n";
            }
        }

        return Observable.throw(modelStateErrors || "Server error");
    }

    protected setUser(user: User): void {
        if (typeof window !== "undefined") {
            localStorage.setItem("user", JSON.stringify(user));
        }
    }

    protected setExpiration(time: Date): void {
        if (typeof window !== "undefined") {
            const expiration = time.getTime() + 30 * 60000;
            localStorage.setItem("expiration", JSON.stringify(expiration));
        }
    }

    protected getHeaders(): HttpHeaders {
        return new HttpHeaders({
            'Content-Type': "application/json",
            'Authorization': `Bearer ${this.getUser().token}`
        });
    }

    getUser(): User {
        const user = new User();
        if (typeof window !== "undefined") {
            const userString = localStorage.getItem("user");
            let userObject = null;
            if (userString != null) {
                userObject = JSON.parse(userString);
            }
            if (userObject != null) {
                user.id = userObject.id;
                user.email = userObject.email;
                user.firstName = userObject.firstName;
                user.lastName = userObject.lastName;
                user.token = userObject.token;
            }
        }
        return user;
    }

    getExpiration(): Date {
        const expiration = new Date();
        if (typeof window !== "undefined") {
            const expirationString = localStorage.getItem("expiration");
            let expirationNumber = 0;
            if (expirationString != null) {
                expirationNumber = parseFloat(expirationString);
            }
            if (expirationNumber > 0) {
                expiration.setTime(expirationNumber);
            }
        }
        return expiration;
    }

    hasToken(): boolean {
        const token = this.getUser().token,
            expired = this.getExpiration().getTime() < Date.now();

        return (typeof token == "string" && token.length > 0 && !expired);
    }

    protected removeUser(): void {
        if (typeof window !== "undefined") {
            localStorage.removeItem("user");
        }
    }

    protected removeExpiration(): void {
        if (typeof window !== "undefined") {
            localStorage.removeItem("expiration");
        }
    }
}