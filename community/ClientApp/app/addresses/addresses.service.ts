import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs/Rx";

import { AppService } from "../app.service";
import { Address } from "./address"

@Injectable()
export class AddressesService extends AppService {

    constructor(private readonly http: HttpClient) {
        super();
    }

    getAddresses(): Observable<Address[]> {

        return this.http
            .get<Address[]>("/addresses/index")
            .catch(this.handleError);
    }

    getAddress(id: string): Observable<Address> {
        
        return this.http
            .get<Address>(`/addresses/details/?id=${id}`)
            .catch(this.handleError);
    }

    createAddress(address: Address): Observable<Address> {

        const that = this,
            body = JSON.stringify(address),
            httpOptions = {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' })
            }

        return that.http
            .post<Address>("/addresses/create", body, httpOptions)
            .catch(that.handleError);
    }
}