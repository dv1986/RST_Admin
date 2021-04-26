import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {

  constructor(private http: HttpClient) {

  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  public UploadMultipleFiles(myFiles, productId): Observable<any> {
    const formData: FormData = new FormData();
    for (var i = 0; i < myFiles.length; i++) {  
      formData.append("Image", myFiles[i]); 
    } 
    return this.http.post<any>(`${environment.contentApiUrl}FileUploader/UploadMultipleFiles`, formData,
    {
      params:
      {
        ProductId: productId,
      }
    });
}


  public upload(image): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('Image', image, image.name);
    return this.http.post<any>(`${environment.contentApiUrl}FileUploader/Upload`, formData);
  }

  public UpdateImage(image, imageName, moduleName, rowId): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('Image', image, image.name);
    formData.append('ImageName', imageName);
    return this.http.post<any>(`${environment.contentApiUrl}FileUploader/UpdateImage`, formData,
      {
        params:
        {
          ImageName: imageName,
          ModuleName: moduleName,
          RowId: rowId
        }
      });
  }

  //#region Product Category Parent
  public getProductCategoryParent(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductCategoryParent`, {}, { params: params });
  }

  public addProductCategoryParent(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductCategoryParent`, request);
  }

  public updateProductCategoryParent(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductCategoryParent`, request);
  }

  public deleteProductCategoryParent(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductCategoryParent`, request);
  }
  //#endregion


  //#region Product Category Parent
  public getProductCategory(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductCategory`, {}, { params: params });
  }

  public addProductCategory(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductCategory`, request);
  }

  public updateProductCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductCategory`, request);
  }

  public deleteProductCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductCategory`, request);
  }

  public GetCategoryLookup(categoryParentId): Observable<any> {
    let params = new HttpParams();
    params = params.append('CategoryParentId', categoryParentId);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetCategoryLookup`, {}, { params: params });
  }
  //#endregion

  //#region Product Sub Category Parent
  public getProductSubCategoryParent(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductSubCategoryParent`, {}, { params: params });
  }

  public addProductSubCategoryParent(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductSubCategoryParent`, request);
  }

  public updateProductSubCategoryParent(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductSubCategoryParent`, request);
  }

  public deleteProductSubCategoryParent(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductSubCategoryParent`, request);
  }

  public getSubCategoryParentLookup(categoryId): Observable<any> {
    let params = new HttpParams();
    params = params.append('CategoryId', categoryId);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetSubCategoryParentLookup`, {}, { params: params });
  }
  //#endregion

  public getProductType(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductType`, {}, { params: params });
  }

  public GetProductTypeLookup(subCategoryId): Observable<any> {
    let params = new HttpParams();
    params = params.append('SubCategoryId', subCategoryId);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductType`, {}, { params: params });
  }

  public addProductType(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductType`, request);
  }

  public updateProductType(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductType`, request);
  }

  public deleteProductType(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductType`, request);
  }

  //#region Produc Sub-Category
  public getProductSubCategory(categoryId): Observable<any> {
    let params = new HttpParams();
    params = params.append('CategoryId', categoryId == null ? 0 : categoryId);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductSubCategory`, {}, { params: params });
  }

  public addProductSubCategory(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductSubCategory`, request);
  }

  public updateProductSubCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductSubCategory`, request);
  }

  public deleteProductSubCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductSubCategory`, request);
  }

  public getSubCategoryLookup(subCategoryParentId): Observable<any> {
    let params = new HttpParams();
    params = params.append('SubCategoryParentId', subCategoryParentId);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetSubCategoryLookup`, {}, { params: params });
  }

  //#endregion

  
  //#region Product Feature Category
  public getProductsFeaturesCategory(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductsFeaturesCategory`, {}, { params: params });
  }

  public addProductsFeaturesCategory(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductsFeaturesCategory`, request);
  }

  public updateProductsFeaturesCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductsFeaturesCategory`, request);
  }

  public deleteProductsFeaturesCategory(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductsFeaturesCategory`, request);
  }
  //#endregion



  //#region Product Feature
  public getProductFeatures(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Categories/GetProductFeatures`, {}, { params: params });
  }

  public addProductFeatures(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Categories/AddProductFeatures`, request);
  }

  public updateProductFeatures(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/UpdateProductFeatures`, request);
  }

  public deleteProductFeatures(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Categories/DeleteProductFeatures`, request);
  }
  //#endregion


  public getBrands(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Product/GetBrands`, {}, { params: params });
  }

  public addBrands(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Product/AddBrands`, request);
  }

  public updateBrands(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateBrands`, request);
  }

  public deleteBrands(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteBrands`, request);
  }




  //#region Product Attribute
  public getProduct(searchStr): Observable<any> {
    let params = new HttpParams();
    params = params.append('SearchStr', searchStr == null ? "" : searchStr);
    return this.http.post<any>(`${environment.baseApiUrl}Product/GetProduct`, {}, { params: params });
  }

  public addProduct(model: any) {
    var request = model;
    return this.http.post<any>(`${environment.baseApiUrl}Product/AddProduct`, request);
  }

  public updateProduct(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateProduct`, request);
  }

  public deleteProduct(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteProduct`, request);
  }
  //#endregion

  public getProductAttributeMapping(productId): Observable<any> {
    let params = new HttpParams();
    params = params.append('ProductId', productId);
    return this.http.post<any>(`${environment.baseApiUrl}Product/GetProductAttributeMapping`, {}, { params: params });
  }

  public updateProductAttributeProductList(model): Observable<any> {
    var request = { ProductAttributes: model.productAttributes, ProductId:model.productId };
    return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateProductAttributeProductList`, request);
  }

  public updateProductAttributeProduct(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/UpdateProductAttributeProduct`, request);
  }

  public deleteProductAttribute(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteProductAttributeMapping`, request);
  }

  public GetProductImageMapping(productId): Observable<any> {
    let params = new HttpParams();
    params = params.append('ProductId', productId);
    return this.http.post<any>(`${environment.baseApiUrl}Product/GetProductImageMapping`, {}, { params: params });
  }

  public deleteProductImageProduct(updatedlist: any[]) {
    var request = { Tasks: updatedlist };
    return this.http.post<any>(`${environment.baseApiUrl}Product/DeleteProductImageProduct`, request);
  }

}