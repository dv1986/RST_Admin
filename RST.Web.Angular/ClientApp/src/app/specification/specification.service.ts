import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SpecificationService {

  constructor(private http: HttpClient) {

  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  //#region Colors
  public getColors(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Specification/GetColors`, {}, { params: params });
  }

  public addColors(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Specification/AddColors`, request);
  }

  public updateColors(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/UpdateColors`, request);
  }

  public deleteColors(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/DeleteColors`, request);
  }
  //#endregion


  //#region Measure Dimension
  public getMeasureDimension(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Specification/GetMeasureDimension`, {}, { params: params });
  }

  public addMeasureDimension(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Specification/AddMeasureDimension`, request);
  }

  public updateMeasureDimension(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/UpdateMeasureDimension`, request);
  }

  public deleteMeasureDimension(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/DeleteMeasureDimension`, request);
  }
  //#endregion


  //#region Product Fabric
  public getProductFabric(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Specification/GetProductFabric`, {}, { params: params });
  }

  public addProductFabric(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Specification/AddProductFabric`, request);
  }

  public updateProductFabric(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/UpdateProductFabric`, request);
  }

  public deleteProductFabric(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/DeleteProductFabric`, request);
  }
  //#endregion


  //#region Product Tag
  public getProductTag(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Specification/GetProductTag`, {}, { params: params });
  }

  public addProductTag(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Specification/AddProductTag`, request);
  }

  public updateProductTag(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/UpdateProductTag`, request);
  }

  public deleteProductTag(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/DeleteProductTag`, request);
  }
  //#endregion


  //#region Product Size Type
  public getProductSizeType(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Specification/GetProductSizeType`, {}, { params: params });
  }

  public addProductSizeType(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Specification/AddProductSizeType`, request);
  }

  public updateProductSizeType(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/UpdateProductSizeType`, request);
  }

  public deleteProductSizeType(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Specification/DeleteProductSizeType`, request);
  }
  //#endregion
}