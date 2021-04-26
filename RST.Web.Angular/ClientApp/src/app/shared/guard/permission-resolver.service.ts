import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class PermissionResolverService implements Resolve<any>  {

  constructor(private authService: AuthenticationService){};
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    console.log('Route Data', route.data);
    //Load the Permissions 
    this.authService.loadUserPermissionForModule(route.data.moduleName)
    
    
  }
  

}
