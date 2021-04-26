import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthGuardService implements CanActivate{

  public href: string = "";
  ngOnInit() {
    this.href = this.router.url;
   
  }
  canActivate(): boolean  {
    if (location.pathname.indexOf("/print-management") >= 0)
      return true;
    if (localStorage.getItem('loginToken')) {
      return true;
  }
    this.router.navigate(['/login']);
    return false;
  }

  constructor(private router: Router) { }

  

}

