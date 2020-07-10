import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth-service.component';
import { Router } from '@angular/router';

@Component({
    selector: 'app-signin-callback',
    template: '<div></div>'
})

export class SigninRedirectCallbackComponent implements OnInit {
    constructor(private authService: AuthService, private router: Router) { }

    ngOnInit() {
        this.authService.completeLogin().then(() => {
            this.router.navigate(['/'], { replaceUrl: true });
        })
    }
}