import { } from "jasmine";
import { defer } from "rxjs/observable/defer";

import { UsersService } from "./users.service";
import { User } from "./user"

let httpClientSpy: { get: jasmine.Spy, post: jasmine.Spy, put: jasmine.Spy, delete: jasmine.Spy };
let usersService: UsersService;
let user1: User, user2: User;

describe("UsersService", () => {

    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj("HttpClient", ["get"]);
        usersService = new UsersService(httpClientSpy as any);
        user1 = { id: "1" };
        user2 = { id: "2" };
    });

    it("index should return a list of users", () => {
        const users: Array<User> = [user1, user2];

        httpClientSpy.get.and.returnValue(defer(() => Promise.resolve(users)));

        usersService
            .index()
            .subscribe(
                res => expect(res).toEqual(users, "expected users"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

    it("details should return a user", () => {
        httpClientSpy.get.and.returnValue(defer(() => Promise.resolve(user1)));

        usersService
            .details("1")
            .subscribe(
                res => expect(res).toEqual(user1, "expected user1"),
                fail);

        expect(httpClientSpy.get.calls.count()).toBe(1, "one call");
    });

});