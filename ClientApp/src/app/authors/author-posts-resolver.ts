import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PostsResolved } from '../posts/post';
import { PostService } from '../posts/post.service';
import { map, catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthorPostsResolver implements Resolve<PostsResolved> {
    constructor(private postService: PostService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<PostsResolved> {
        const id = route.parent.paramMap.get('id');
        const page = route.paramMap.get('page');
        if (isNaN(+page)) {
            const message = "Invalid page number";
            console.error(message);
            return of({ posts: null, error: message });
        }

        return this.postService.getPosts(+page,id).pipe(
            map(posts => ({ posts: posts })),
            catchError(error => {
                const message = 'Retrieval error';
                console.error(message);
                return of({ posts: null, error: message });
            })
        );
    }
}