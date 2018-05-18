import { } from "jasmine";
import { defer as observableDefer } from "rxjs";

import { AddressesService } from "./addresses.service";
import { Address } from "./address"

let httpClientSpy: { get: jasmine.Spy, post: jasmine.Spy, put: jasmine.Spy, delete: jasmine.Spy };
let addressesService: AddressesService;
let address1: Address, address2: Address;

describe("AddressesService", () => {

    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj("HttpClient", ["get", "post", "put", "delete"]);
        addressesService = new AddressesService(httpClientSpy as any);
        address1 = { id: "1" };
        address2 = { id: "2" };
    });

    it("index should return a list of addresses", () => {
        const addresses: Array<Address> = [address1, address2];

        httpClientSpy.get.and.returnValue(observableDefer(() => Promise.resolve(addresses)));

        addressesService
            .index()
            .subscribe(
                res => expect(res).toEqual(addresses, "expected addresses"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

    it("details should return an address", () => {
        httpClientSpy.get.and.returnValue(observableDefer(() => Promise.resolve(address1)));

        addressesService
            .details("1")
            .subscribe(
                res => expect(res).toEqual(address1, "expected address1"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

    it("create should return an address", () => {
        httpClientSpy.post.and.returnValue(observableDefer(() => Promise.resolve(address1)));

        addressesService
            .create(address1)
            .subscribe(
                res => expect(res).toEqual(address1, "expected address1"),
                fail);

        expect(httpClientSpy.post.calls.count()).toBe(1, "one call");
    });

    it("edit should not return anything", () => {
        httpClientSpy.put.and.returnValue(observableDefer(() => Promise.resolve()));

        addressesService
            .edit(address1)
            .subscribe(
                res => expect(res).toBeUndefined("expected undefined"),
                fail);

        expect(httpClientSpy.put.calls.count()).toBe(1, "one call");
    });

    it("delete should not return anything", () => {
        httpClientSpy.delete.and.returnValue(observableDefer(() => Promise.resolve()));

        addressesService
            .delete(address1.id)
            .subscribe(
                res => expect(res).toBeUndefined("expected undefined"),
                fail);

        expect(httpClientSpy.delete.calls.count()).toBe(1, "one call");
    });

});