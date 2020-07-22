import { NgModule } from '@angular/core';
import { SigninRedirectCallbackComponent } from './signin-redirect-callback.component';
import { CoreRoutingModule } from './core-routing.module';
import { SignoutRedirectCallbackComponent } from './signout-redirect-callback.components';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthIncterceptorService } from './auth-interceptor.service';


@NgModule({
    imports: [CoreRoutingModule],
    exports: [],
    declarations: [
        SigninRedirectCallbackComponent,
        SignoutRedirectCallbackComponent,
    ],
    providers: [ {provide: HTTP_INTERCEPTORS, useClass: AuthIncterceptorService, multi: true}],
})
export class CoreModule { }
