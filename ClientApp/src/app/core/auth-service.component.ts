import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client'
import { Constants } from '../constants';
import { Subject } from 'rxjs';
import { AuthorService } from '../authors/author-service';
import { AuthorCreateViewModel } from '../authors/author';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private userManager: UserManager;
    private user: User;
    private loginChangedSubject = new Subject<boolean>();

    loginChanged = this.loginChangedSubject.asObservable();

    constructor(private authorService: AuthorService) {
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

    async isLoggedIn(): Promise<boolean> {
        const user = await this.userManager.getUser();
        const userCurrent = !!user && !user.expired;
        if (this.user !== user) {
            this.loginChangedSubject.next(userCurrent);
        }
        this.user = user;
        return userCurrent;
    }

    async completeLogin() {
        const user = await this.userManager.signinRedirectCallback();
        this.user = user;
        this.loginChangedSubject.next(!!user && !user.expired);
        const authorViewModel = this.createAuthorViewModel();
        this.authorService.relateUserToAuthor(authorViewModel);
        // return user;
    }

    createAuthorViewModel() {
        let newAuthor = new AuthorCreateViewModel();
        newAuthor.id = this.user.profile.sub;
        newAuthor.displayName = this.user.profile.name;
        return newAuthor;
    }

    logout() {
        this.userManager.signoutRedirect();
    }

    completeLogout() {
        this.user = null;
        return this.userManager.signoutRedirectCallback();
    }

    async getAccessToken() {
        const user = await this.userManager.getUser();
        if (!!user && !user.expired) {
            return user.access_token;
        }
        else {
            return null;
        }
    }
}