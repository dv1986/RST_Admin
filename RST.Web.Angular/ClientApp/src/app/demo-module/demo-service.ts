import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class DemoModuleService {

    constructor(private http: HttpClient) { }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

    public GetProcedureParameters(ProcedureName) {
        return this.http.get<any>(`${environment.baseApiUrl}CodeGenerator/GetProcedureParameters`, { params: new HttpParams().set('ProcedureName', ProcedureName) });
    }
    public GenerateCode(ProcedureName, Parameters) {
        return this.http.post<any>(`${environment.baseApiUrl}CodeGenerator/GenerateCode`, { ProcedureName: ProcedureName, Parameters: Parameters });
    }

    public GenerateFunction(ProcedureName, type) {
        return this.http.get<any>(`${environment.baseApiUrl}CodeGenerator/GenerateDataInsertFunction`, { params: new HttpParams().set('ProcedureName', ProcedureName).append('type', type) });
    }
}