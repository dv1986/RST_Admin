import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class LayoutServiceService {

    constructor(private http: HttpClient) { }
    public GetWebRootMenu(): Observable<any> {
        return this.http.get<any>(`${environment.baseApiUrl}Module/GetWebRootMenu`);
    }
    public GetChildModules(UserName, MenuId): Observable<any> {
        return this.http.get<any>(`${environment.baseApiUrl}Module/GetChildModules`, { params: { UserName: UserName, MenuId: MenuId } });
    }
    // public LoadSystemConfigs(): Observable<any> {
    //     var SystemConfigs = {
    //         CCcustomerUserName: environment.CCcustomerUserName,
    //         CCcustomerPassword: environment.CCcustomerPassword,
    //         CCcustomerMerchant: environment.CCcustomerMerchant,
    //         CCKey: environment.CCKey,
    //         CrossDockExceptions: environment.CrossDockExceptions,
    //         CrossDockWarehouses: environment.CrossDockWarehouses,
    //         DiaryOrderSplit: environment.DiaryOrderSplit,
    //         SOStatus07AcctList: environment.SOStatus07AcctList,
    //         DelFeeExceptAccount: environment.DelFeeExceptAccount,
    //         DelFeeExceptParent: environment.DelFeeExceptParent,
    //         DelFeeExceptContract: environment.DelFeeExceptContract,
    //         EmailServer: environment.EmailServer,
    //         Environment: environment.Environment,
    //         PayPalClientId: environment.PayPalClientId,
    //         PayPalSecret: environment.PayPalSecret,
    //         PaypalMode: environment.PaypalMode,
    //         SearchServers: environment.SearchServers,
    //         TonerBoxCode: environment.TonerBoxCode,
    //         CatCode: environment.CatCode,
    //         FurnitureCatCode: environment.FurnitureCatCode,
    //         DiaryCatCode: environment.DiaryCatCode
    //     };

    //     return this.http.post<any>(`${environment.baseApiUrl}Checkout/LoadSystemConfig`, SystemConfigs);
    // }
}
