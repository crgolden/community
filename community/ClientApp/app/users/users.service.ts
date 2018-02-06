import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { User } from "./user"

@Injectable()
export class UsersService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    getUsers(): Observable<User[]> {

        return this.http
            .get<User[]>("/users/index")
            .catch(this.handleError);
    }

    getUser(id: string): Observable<User> {
        
        return this.http
            .get<User>(`/users/details/?id=${id}`)
            .catch(this.handleError);
    }
}