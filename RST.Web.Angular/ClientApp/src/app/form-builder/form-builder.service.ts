import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class FormBuilderService {

    constructor(private http: HttpClient) {

    }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

    //#region Product Category Parent
    public getFormBuilder(id): Observable<any> {
        let params = new HttpParams();
        params = params.append('Id', id);
        return this.http.post<any>(`${environment.baseApiUrl}FormBuilder/GetFormBuilder`, {}, { params: params });
    }

    public addFormBuilder(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}FormBuilder/AddFormBuilder`, request);
    }

    public updateFormBuilderbyId(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}FormBuilder/UpdateFormBuilderbyId`, request);
    }

    public updateFormBuilder(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}FormBuilder/UpdateFormBuilder`, request);
    }

    public deleteFormBuilder(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}FormBuilder/DeleteFormBuilder`, request);
    }
    //#endregion
}