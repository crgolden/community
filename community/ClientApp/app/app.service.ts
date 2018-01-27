import { Injectable } from "@angular/core"
import { Observable } from "rxjs/Rx";

@Injectable()
export class AppService {

    protected handleError(error: any) {
        const applicationError = error.headers.get("Application-Error");

        if (applicationError) {
            return Observable.throw(applicationError);
        }

        let modelStateErrors: string = "";
        const serverError = error.json();

        if (!serverError.type) {
            for (let key in serverError) {
                if (serverError.hasOwnProperty(key) && serverError[key])
                    modelStateErrors += serverError[key] + "\n";
            }
        }

        return Observable.throw(modelStateErrors || "Server error");
    }
}