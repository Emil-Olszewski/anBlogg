import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Post } from './post';
import { Constants } from '../constants';
import { Observable } from 'rxjs';
import { AuthorService } from '../authors/author-service';

@Injectable({ providedIn: 'root' })
export class PostService {
  private postsUrl = Constants.API_URL + 'posts/';
  private authorsUrl = Constants.API_URL + 'authors/';

  constructor(private http: HttpClient, private authorService: AuthorService) {}

  getPosts(page: number, authorId: string = null) {
    
    const options = {
      observe: 'response' as const,
      params: {
        pageNumber: page,
        pageSize: 5
      }
    };
    
    const url = authorId ? this.authorsUrl + authorId + '/posts' : this.postsUrl;

    return this.http.get<Post[]>(url, <any>options).pipe(
      catchError(this.handleError)
    ) as Observable<any>;
  }

  getPost(postId: string) {
      return this.http.get<Post>(this.postsUrl + postId).pipe(
          catchError(this.handleError)
      ) as Observable<Post>;
  }

  postPost(post: Post) {
    const authorId = this.authorService.loggedAuthorId;
    const url =  this.authorsUrl + authorId + '/posts';
    return this.http.post<Post>(url, post).pipe(
      catchError(this.handleError)
    ) as Observable<Post>;
  }

  handleError(handleError: any): any {
    console.log(handleError);
  }
}
