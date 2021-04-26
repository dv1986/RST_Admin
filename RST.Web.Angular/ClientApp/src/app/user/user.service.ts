import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUser } from './IUser';

@Injectable({
    providedIn: 'root',
})
export class UserService {

    constructor(private http: HttpClient) {

    }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }


    public addUser(user: IUser): Observable<any> {
        return this.http.post<any>(`${environment.baseApiUrl}user/AddUser`, user);
    }

    public addBulkUser(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}user/AddBulkUser`, request);
    }


    public getAllUsers(userName: any): Observable<any> {
        let params = new HttpParams();
        params = params.append('UserName', userName);
        return this.http.post<any>(`${environment.baseApiUrl}user/GetUsers`, {}, { params: params });
    }

    public updateUser(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}user/UpdateUser`, request);
    }

    public deleteUser(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}user/DeleteUser`, request);
    }

    public getDemo(searchString: any): Observable<any> {
        let params = new HttpParams();
        params = params.append('searchString', searchString);
        return this.http.post<any>(`${environment.baseApiUrl}demo/getAll`, {}, { params: params });
    }

    public getUserPermissionforUser(userId: any): Observable<any> {
        let params = new HttpParams();
        params = params.append('UserId', userId);
        return this.http.post<any>(`${environment.baseApiUrl}user/GetUserPermissionforUser`, {}, { params: params });
    }

    public addUserPermission(userId: number, subMenuIds: any[]) {
        var request = { "UserId": userId, "SubMenuIds": subMenuIds };
        return this.http.post<any>(`${environment.baseApiUrl}user/AddUserPermission`, request);
    }

    public getUserType(): Observable<any> {
        let params = new HttpParams();
        return this.http.post<any>(`${environment.baseApiUrl}user/GetUserType`, {}, { params: params });
    }

}