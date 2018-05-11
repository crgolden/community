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
            .get<User[]>("/api/v1/Users/Index")
            .catch(this.handleError);
    }

    details(id: string | null): Observable<string | User> {

        return this.http
            .get<User>(`/api/v1/Users/Details/${id}`)
            .catch(this.handleError);
    }
}