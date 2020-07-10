import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of, combineLatest } from 'rxjs';
import { AuthorResolved } from './author';
import { AuthorService } from './author-service';
import { map, catchError } from 'rxjs/operators';
import { PostService } from '../posts/post.service';

@Injectable({ providedIn: 'root' })
export class AuthorResolver implements Resolve<AuthorResolved> {
    constructor(private authorService: AuthorService, private postSevice: PostService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<AuthorResolved> {
        const id = route.paramMap.get('id');

        return this.authorService.getAuthor(id).pipe(
            map(author => ({ author: author })),
            catchError(error => {
                const message = 'Retrieval error';
                console.error(message);
                return of({ author: null, error: message });
            })
        );
    }
}