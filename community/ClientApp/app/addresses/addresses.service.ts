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

    index(): Observable<string |Address[]> {

        return this.http
            .get<Address[]>("/Addresses/Index")
            .catch(this.handleError);
    }

    details(id: string | null): Observable<string | Address> {
        
        return this.http
            .get<Address>(`/Addresses/Details/?id=${id}`)
            .catch(this.handleError);
    }

    create(address: Address): Observable<string> {

        const body = JSON.stringify(address),
            options = { headers: this.getHeaders() };

        return this.http
            .post<string>("/Addresses/Create", body, options)
            .catch(this.handleError);
    }

    edit(address: Address) {
        const body = JSON.stringify(address),
            options = { headers: this.getHeaders() };

        return this.http
            .put(`/Addresses/Edit?id=${address.id}`, body, options)
            .catch(this.handleError);
    }

    delete(id: string | undefined) {
        const options = { headers: this.getHeaders() };

        return this.http
            .delete(`/Addresses/Delete?id=${id}`, options)
            .catch(this.handleError);
    }
}