import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class MasterService {

    constructor(private http: HttpClient) {

    }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

   

    public getCountryLookup(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetCountryLookup`, {}, { params: params });
    }
    
    public getCountry(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetCountry`, {}, { params: params });
    }

    public addCountry(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/AddCountry`, request);
    }

    public updateCountry(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/UpdateCountry`, request);
    }

    public deleteCountry(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/DeleteCountry`, request);
    }

    public getState(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetState`, {}, { params: params });
    }

    // public getStateLookup(searchStr): Observable<any> {
    //     let params = new HttpParams();
    //     params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    //     return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetStateLookup`, {}, { params: params });
    // }
    public getStateLookup(countryId): Observable<any> {
        let params = new HttpParams();
        params = params.append('CountryId', countryId);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetStateLookup`, {}, { params: params });
    }

    public addState(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/AddState`, request);
    }

    public updateState(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/UpdateState`, request);
    }

    public deleteState(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/DeleteState`, request);
    }

    public getCity(countryId, stateId): Observable<any> {
        let params = new HttpParams();
        params = params.append('CountryId', countryId);
        params = params.append('StateId', stateId);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetCity`, {}, { params: params });
    }

    public getCityLookup(stateId): Observable<any> {
        let params = new HttpParams();
        params = params.append('StateId', stateId);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetCityLookup`, {}, { params: params });
    }

    public addCity(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/AddCity`, request);
    }

    public updateCity(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/UpdateCity`, request);
    }

    public deleteCity(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/DeleteCity`, request);
    }



    public getMenu(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetMenu`, {}, { params: params });
    }

    public addMenu(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/AddMenu`, request);
    }

    public updateMenu(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/UpdateMenu`, request);
    }

    public deleteMenu(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/DeleteMenu`, request);
    }

    public getSubMenu(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetSubMenu`, {}, { params: params });
    }

    public addSubMenu(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/AddSubMenu`, request);
    }

    public updateSubMenu(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/UpdateSubMenu`, request);
    }

    public deleteSubMenu(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/DeleteSubMenu`, request);
    }

    public getSeoContent(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}SEO/GetSeoContent`, {}, { params: params });
    }

    public addSeoContent(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}SEO/AddSeoContent`, request);
    }

    public updateSeoContent(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}SEO/UpdateSeoContent`, request);
    }

    public deleteSeoContent(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}SEO/DeleteSeoContent`, request);
    }

    public getSubscriptionList(): Observable<any> {
        let params = new HttpParams();
        return this.http.post<any>(`${environment.baseApiUrl}Lookup/GetSubscriptionList`, {}, { params: params });
    }
}