import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Configuration } from "tslint";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent {
  private readonly _http: HttpClient;
  private readonly _baseUrl: string;
  public isProcessing = false;

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
  }

  public startProcessing() {
    this.isProcessing = true;
    this._http.post<object>(this._baseUrl + "process", null).subscribe(
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

export class Config {
  intervalMinutes: number;
}
