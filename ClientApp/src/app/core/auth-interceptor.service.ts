import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { AuthService } from './auth-service.component';
import { Constants } from '../constants';

@Injectable({ providedIn: 'root' })
export class AuthIncterceptorService implements HttpInterceptor {
    constructor(private authService: AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if(req.url.startsWith(Constants.API_URL)){
            return from(this.authService.getAccessToken().then(token => {
                if(!token) {
                    return next.handle(req).toPromise();
                }
                let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
                headers = headers.set('Content-Type', 'application/json');
                const authReq = req.clone({ headers });
                return next.handle(authReq).toPromise();
            }));
        } else {
            return next.handle(req);
        }
        
    }
}