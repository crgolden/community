import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { Address } from "./address"

@Injectable()
export class AddressesService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    index(): Observable<string | Address[]> {

        return this.http
            .get<Address[]>("/api/v1/Addresses/Index")
            .catch(this.handleError);
    }

    details(id: string | null): Observable<string | Address> {

        return this.http
            .get<Address>(`/api/v1/Addresses/Details/${id}`)
            .catch(this.handleError);
    }

    create(address: Address): Observable<string | Address> {

        const body = JSON.stringify(address),
            options = { headers: this.getHeaders() };

        return this.http
            .post<Address>("/api/v1/Addresses/Create", body, options)
            .catch(this.handleError);
    }

    edit(address: Address) {
        const body = JSON.stringify(address),
            options = { headers: this.getHeaders() };

        return this.http
            .put(`/api/v1/Addresses/Edit/${address.id}`, body, options)
            .catch(this.handleError);
    }

    delete(id: string | undefined) {
        const options = { headers: this.getHeaders() };

        return this.http
            .delete(`/api/v1/Addresses/Delete/${id}`, options)
            .catch(this.handleError);
    }
}