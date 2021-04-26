import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    

    const authService = this.injector.get(AuthenticationService);
    const authHeader = authService.getAuthorizationHeader();
    // Clone the request to add the new header.
    const authReq = req.clone({ headers: req.headers.set('Authorization', authHeader) });
    // Pass on the cloned request instead of the original request.
    return next.handle(authReq);
  }

  constructor(private injector: Injector) { }

}
