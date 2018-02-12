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

    index(): Observable<Address[] | string> {

        return this.http
            .get<Address[]>("/Addresses/Index")
            .catch(this.handleError);
    }

    details(id: string): Observable<Address[] | Address | string> {
        
        return this.http
            .get<Address>(`/Addresses/Details/?id=${id}`)
            .catch(this.handleError);
    }

    create(address: Address): Observable<Address[] | Address | string> {

        const that = this,
            body = JSON.stringify(address),
            options = { headers: that.getHeaders() };

        return that.http
            .post<Address>("/Addresses/Create", body, options)
            .catch(that.handleError);
    }

    edit(address: Address): Observable<Address[] | Address | string> {
        const that = this,
            body = JSON.stringify(address),
            options = { headers: that.getHeaders() };

        return that.http
            .post<Address>(`/Addresses/Edit?id=${address.id}`, body, options)
            .catch(that.handleError);
    }

    delete(id: string): Observable<Address[] | boolean | string> {
        const that = this,
            options = { headers: that.getHeaders() };

        return that.http
            .post<boolean>(`/Addresses/Delete?id=${id}`, {}, options)
            .catch(that.handleError);
    }

    isAddress(address: Address[] | Address | string): address is Address {
        return address as Address !== undefined;
    }
}