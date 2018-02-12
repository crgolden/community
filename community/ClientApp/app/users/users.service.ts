import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { User } from "./user"

@Injectable()
export class UsersService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    index(): Observable<string | User[]> {

        return this.http
            .get<User[]>("/users/index")
            .catch(this.handleError);
    }

    details(id: string): Observable<string | User> {
        
        return this.http
            .get<User>(`/Users/Details/?id=${id}`)
            .catch(this.handleError);
    }

    isUser(user: User[] | User | string): user is User {
        return user as User !== undefined;
    }
}