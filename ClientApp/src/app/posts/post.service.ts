import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Post } from './post';
import { Constants } from '../constants';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PostService {
  private postsUrl = Constants.API_URL + 'posts/';
  private authorsUrl = Constants.API_URL + 'authors/';

  constructor(private http: HttpClient) {}

  getPosts(page: number, authorId: string = null) {
    const requestBody = {
      params: {
        pageNumber: page > 0 ? page : 1,
        postsDisplayed: 10,
      },
    };
    const url = authorId ? this.authorsUrl + authorId + '/posts' : this.postsUrl;
    return this.http.get<Post[]>(url, <any>requestBody).pipe(
      //tap(data => console.log(JSON.stringify(data))),
      catchError(this.handleError)
    ) as Observable<Post[]>;
  }

  getPost(postId: string) {
      return this.http.get<Post>(this.postsUrl + postId).pipe(
          catchError(this.handleError)
      ) as Observable<Post>;
  }

  postPost(authorId: string, post: Post) {
    const url =  this.authorsUrl + authorId + '/posts';
    return this.http.post<Post>(url, post).pipe(
      catchError(this.handleError)
    ) as Observable<Post>;
  }

  handleError(handleError: any): any {
    console.log(handleError);
  }
}
