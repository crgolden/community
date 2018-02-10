import { Injectable } from "@angular/core"
import { HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { User } from "./users/user"

@Injectable()
export class AppService {

    readonly user: User;
    readonly headers: HttpHeaders;

    constructor() {
        this.user = this.getUser();
        this.headers = new HttpHeaders({
            'Content-Type': "application/json",
            'Authorization': `Bearer ${this.user.token}`
        });
    }

    protected handleError(error: any) {
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

    protected setUser(user: User) {
        if (typeof window !== "undefined") {
            localStorage.setItem("user", JSON.stringify(user));
        }
    }

    protected removeUser() {
        if (typeof window !== "undefined") {
            localStorage.removeItem("user");
        }
    }

    private getUser(): User {
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
}