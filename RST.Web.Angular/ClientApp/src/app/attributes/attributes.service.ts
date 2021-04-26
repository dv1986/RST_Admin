import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class AttributeService {

    constructor(private http: HttpClient) {

    }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

    //#region Product Attribute Parent
    public getproductAttributeParent(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Product/GetproductAttributeParent`, {}, { params: params });
    }

    public getProductAttributeParentbyProdcutType(productTypeId): Observable<any> {
        let params = new HttpParams();
        params = params.append('ProductTypeId', productTypeId);
        return this.http.post<any>(`${environment.baseApiUrl}Product/GetProductAttributeParentbyProdcutType`, {}, { params: params });
    }

    public addproductAttributeParent(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Product/AddproductAttributeParent`, request);
    }

    public updateproductAttributeParent(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateproductAttributeParent`, request);
    }

    public deleteproductAttributeParent(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteproductAttributeParent`, request);
    }
    //#endregion


    //#region Product Attribute
    public getproductAttribute(searchStr): Observable<any> {
        let params = new HttpParams();
        params = params.append('SearchStr', searchStr == null ? "" : searchStr);
        return this.http.post<any>(`${environment.baseApiUrl}Product/GetproductAttribute`, {}, { params: params });
    }

    public addproductAttribute(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Product/AddproductAttribute`, request);
    }

    public updateproductAttribute(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateproductAttribute`, request);
    }

    public deleteproductAttribute(updatedlist: any[]) {
        var request = { Tasks: updatedlist };
        return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteproductAttribute`, request);
    }
    //#endregion

    public addAttributeProductTypeMapping(model: any) {
        var request = model;
        return this.http.post<any>(`${environment.baseApiUrl}Product/AddAttributeProductTypeMapping`, request);
    }

    public getAllAttributeProductTypeMapping(): Observable<any> {
        let params = new HttpParams();
        return this.http.post<any>(`${environment.baseApiUrl}Product/GetAllAttributeProductTypeMapping`, {}, 
        { params: params });
    }

    public getAttributeProductTypeMapping(categoryParentId, categoryId, subCategoryParentId, subCategoryId, productTypeId): Observable<any> {
        let params = new HttpParams();
        params = params.append('CategoryParentId', categoryParentId);
        params = params.append('CategoryId', categoryId);
        params = params.append('SubCategoryParentId', subCategoryParentId);
        params = params.append('SubCategoryId', subCategoryId);
        params = params.append('ProductTypeId', productTypeId);
        return this.http.post<any>(`${environment.baseApiUrl}Product/GetAttributeProductTypeMapping`, {}, 
        { params: params });
    }
}