import { Injectable } from '@angular/core';

import { User } from './user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    currentUser: User | null;
    redirectUrl: string;

    constructor(private http: HttpClient) { }
    httpOptions = {  
        headers: new HttpHeaders({  
          'Content-Type': 'application/json'  
        })  
      } 


    public validateUser(userName: string, password: string): Observable<any> {

        password = encodeURIComponent(password);

        return this.http.post(`${environment.baseApiUrl}authentication/Authenticate/?userRef=${userName}&password=${password}`
            , null
        );
        
    }

    public getPermissions(moduleName: string): Observable<any> {
        return this.http.post(`${environment.baseApiUrl}authentication/GetPermissions/?userName=${this.getLogedUserName()}&moduleName=${moduleName}`
            , null
        );
    }

    public loadUserPermissionForModule(moduleName: string) {
        this.getPermissions(moduleName).subscribe(response => {
            if (response.state == 0) {
                console.log(response);

                localStorage.setItem(`PER_${this.getLogedUserName()}_${moduleName}`, JSON.stringify(response.data));
            }
        });
    }

    public isAuthorized(moduleName: string, checkPoint: string) {
        let storageKey = `PER_${this.getLogedUserName()}_${moduleName}`;
        if (localStorage.getItem(storageKey) !== null) {
            let permissions = JSON.parse(localStorage.getItem(storageKey));
            console.log('Find Permission', (permissions as Array<string>).indexOf(checkPoint));

            if (permissions && permissions.length > 0 && (permissions as Array<string>).indexOf(checkPoint) >= 0) {
                return true;
            }
        }
        return false;
    }

    public logout(): void {
        localStorage.removeItem("loginToken");
        localStorage.removeItem("userName");
    }

    public loginUser(userName: string, authToken: string): void {
        localStorage.setItem('loginToken', authToken);

        localStorage.setItem('userName', this.getLogedUserName());
    }

    public getLogedUser(): string {
        if (localStorage.getItem("loginToken") !== null) {
            let decodedToken = this.decodeToken(localStorage.getItem("loginToken"))
            return `${decodedToken.fullName} - ${decodedToken.dept}`;
        }
        else {
            return "";
        }
    }

    public getLogedUserDept(): string {
        if (localStorage.getItem("loginToken") !== null) {
            let decodedToken = this.decodeToken(localStorage.getItem("loginToken"))
            return `${decodedToken.dept}`;
        }
        else {
            return "";
        }
    }

    public getLogedUserName(): string {
        //
        if (localStorage.getItem("loginToken") !== null) {
            let decodedToken = this.decodeToken(localStorage.getItem("loginToken"))
            return `${decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}`;
        }
        else {
            return "";
        }
    }

    public getLogedEmail(): string {
        //
        if (localStorage.getItem("loginToken") !== null) {
            let decodedToken = this.decodeToken(localStorage.getItem("loginToken"))
            return `${decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]}`;
        }
        else {
            return "";
        }
    }

    public getLogedUserId(): string {
        //
        if (localStorage.getItem("loginToken") !== null) {
            let decodedToken = this.decodeToken(localStorage.getItem("loginToken"))
            return `${decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]}`;
        }
        else {
            return "";
        }
    }

    public getAuthorizationHeader() {
        if (localStorage.getItem("loginToken") !== null) {
            return "Bearer " + localStorage.getItem("loginToken");
        }

        return "";
    }



    private decodeToken(token: string) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        var decoded = window.atob(base64);
        if (!decoded) {
            throw new Error('Cannot decode the token');
        }
        return JSON.parse(decoded);
    }

    isLoggedIn(): boolean {
        return !!this.currentUser;
    }

    login(userName: string, password: string): void {
        // Code here would log into a back end service
        // and return user information
        // This is just hard-coded here.
        this.currentUser = {
            id: 2,
            userName,
            isAdmin: false
        };
    }
}
