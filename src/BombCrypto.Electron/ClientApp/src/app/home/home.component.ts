import { Component, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Configuration } from "tslint";
import { ConfigEntity } from "../config-entity";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  private readonly _http: HttpClient;
  private readonly _baseUrl: string;
  public isProcessing = false;

  configModel = new ConfigEntity(3);

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
  }

  public startProcessing() {
    this.isProcessing = true;
    console.log(JSON.stringify(this.configModel));
    this._http
      .post<object>(this._baseUrl + "process", this.configModel, {
        headers: new HttpHeaders({ "Content-Type": "application/json" }),
      })
      .subscribe(
        (result) => {
          console.log(result);
        },
        (error) => console.error(error)
      );
  }

  public stopProcessing() {
    this.isProcessing = false;
    this._http.post<object>(this._baseUrl + "process/stop", null).subscribe(
      (result) => {
        console.log(result);
      },
      (error) => console.error(error)
    );
  }
}
