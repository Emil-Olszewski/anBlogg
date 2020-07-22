import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PostResolved, Post } from './post';
import { PostService } from './post.service';
import { map, catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class PostResolver implements Resolve<PostResolved> {
  constructor(private postService: PostService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<PostResolved> {
    const id = route.paramMap.get('id');
    if (id === '0') {
      return of({ post: new Post() });
    }
    
    return this.postService.getPost(id).pipe(
      map((post) => ({ post: post })),
      catchError((error) => {
        const message = 'Retrieval error: + ' + error;
        console.error(message);
        return of({ post: null, error: message });
      })
    );
  }
}
