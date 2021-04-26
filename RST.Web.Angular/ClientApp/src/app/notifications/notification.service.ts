import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PushNotificationService {

  constructor(private http: HttpClient) {

  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  //#region Notification
  public getNotification(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Notification/GetNotification`, {}, { params: params });
  }

  public updateNotification(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Notification/UpdateNotification`, request);
  }

  public deleteNotification(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Notification/DeleteNotification`, request);
  }
  //#endregion
}