import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Service } from "src/app/models/enum";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { AllInOneRequest } from "../models/aio";
import { UUID } from "angular2-uuid";

@Injectable({
  providedIn: "root",
})
export class MainService {
  isProcessing = false;
  headerDict = {
    "Content-Type": "application/json;",
    Accept: "*/*",
    "Access-Control-Allow-Origin": "*",
  };
  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string
  ) {}

  verifySessionID(deviceID: string, sessionID: string) {
    let req = this.newRequest("", deviceID, sessionID);

    console.log(req);

    return this.http.post(this.baseUrl + "aio/verify-sessionId", req, {
      headers: new HttpHeaders(this.headerDict),
    });
  }

  uploadImage(img: string, type: string, deviceID: string, sessionID: string) {
    let req = this.newRequest(
      { imageBas64: img, imageType: type },
      deviceID,
      sessionID
    );
    return this.http.post(this.baseUrl + "aio/upload-image", req, {
      headers: new HttpHeaders(this.headerDict),
    });
  }

  newRequest(
    data: any = null,
    deviceID: string,
    sessionID: string,
    isProcessing: boolean = true
  ) {
    this.isProcessing = isProcessing;
    let req = new AllInOneRequest<typeof data>();

    req.refNumber = UUID.UUID();
    req.refDateTime = new Date().toISOString().replace("Z", "");
    req.deviceID = deviceID;
    req.sessionID = sessionID;
    req.data = data;

    return JSON.stringify(req);
  }
}
