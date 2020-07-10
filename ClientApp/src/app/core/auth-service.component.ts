import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client'
import { Constants } from '../constants';
import { Observable, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private userManager: UserManager;
    private user: User;
    private loginChangedSubject = new Subject<boolean>();

    loginChanged = this.loginChangedSubject.asObservable();

    constructor() {
        const stsSettings = {
            authority: Constants.stsAuthority,
            client_id: Constants.clientId,
            redirect_uri: `${Constants.clientRoot}signin-callback`,
            scope: 'openid profile anBloggApi',
            response_type: 'code',
            post_logout_redirect_uri: `${Constants.clientRoot}signout-callback`
        };

        this.userManager = new UserManager(stsSettings);
    }

    login() {
        return this.userManager.signinRedirect();
    }

    isLoggedIn(): Promise<boolean> {
        return this.userManager.getUser().then(user => {
            const userCurrent = !!user && !user.expired;
            if (this.user !== user) {
                this.loginChangedSubject.next(userCurrent);
            }
            this.user = user;
            return userCurrent;
        })
    }

    completeLogin() {
        return this.userManager.signinRedirectCallback().then(user => {
            this.user = user;
            this.loginChangedSubject.next(!!user && !user.expired);
            console.log(this.user);
            return user;
        })
    }

    logout() {
        this.userManager.signoutRedirect();
    }

    completeLogout() {
        this.user = null;
        return this.userManager.signoutRedirectCallback();
    }

    getAccessToken() {
        return this.userManager.getUser().then(user => {
            if (!!user && !user.expired) {
                return user.access_token
            } else {
                return null;
            }
        })
    }
}