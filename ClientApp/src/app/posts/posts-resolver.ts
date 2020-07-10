import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PostService } from './post.service';
import { PostsResolved } from './post';
import { catchError, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class PostsResolver implements Resolve<PostsResolved> {
    constructor(private postService: PostService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<PostsResolved> {
        const authorId = route.parent.paramMap.get('id');
        const page = route.paramMap.get('page');

        if (isNaN(+page)) {
            const message = "Invalid page number";
            console.error(message);
            return of({ posts: null, error: message });
        }

        return this.postService.getPosts(+page,authorId).pipe(
            map(posts => ({ posts: posts })),
            catchError(error => {
                const message = 'Retrieval error: + ' + error;
                console.error(message);
                return of({ posts: null, error: message });
            })
        );
    }
}