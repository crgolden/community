import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot } from "@angular/router";

import { AddressesService } from "../addresses.service"
import { Address } from "../address"

@Injectable()
export class DetailsResolver implements Resolve<string | Address> {

    constructor(private readonly addressesService: AddressesService) {
    }

    resolve(route: ActivatedRouteSnapshot) {
        return this.addressesService.details(route.paramMap.get("id"));
    }
}