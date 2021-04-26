import { Injectable , Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpSentEvent, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NotificationService } from '../services/notification.service';
import {  Router } from '@angular/router';
import { SpinnerService } from '../services/spinner.service';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptorService  implements HttpInterceptor {
  
  constructor(private injector: Injector,private router: Router,private spinnerService:SpinnerService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const toaster = this.injector.get(NotificationService);   
    return next.handle(req).pipe(
      tap((ev: HttpEvent<any>) => {
        if (ev instanceof HttpResponse) {
         
          if(ev.body && ev.body.state)
          {
            console.log(ev.headers["Content-Type"]);
            toaster.ProcessResponse( ev.body);
          }
        }
      }),
      catchError((response: any) => {
      
        if (response instanceof HttpErrorResponse) {    
          console.log(response);
          if(response.status == 401) //Session Expired redirect to login 
          {
              localStorage.removeItem("loginToken");
              this.router.navigate(['/login']);
              toaster.ShowError("Session Expired",6000);
          }  
        if(response.error) 
        {
            if (response.message == "Http failure response for (unknown url): 0 Unknown Error")
                toaster.ShowError("There is a release in progress. Try again in 30 seconds. If problem persists, contact IT");
            else
                toaster.ShowError( response.message);
        } 
        else{ //No Error body in the response    
          console.log('Should Process error')      
          toaster.ProcessResponse(response)    ;         
        } 
        console.log('Should Hide error')
        //Hide any progress 
        this.spinnerService.hide();
      }
      return Observable.throw(response);
    })
    )
  }
}
