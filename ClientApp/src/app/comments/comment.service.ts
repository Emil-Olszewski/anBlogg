import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../constants';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class CommentService {
    private authorsUrl = Constants.API_URL + 'authors/';
    
    constructor(private http: HttpClient) { }
    
    getComments(authorId: string, postId: string) {
        const url = this.authorsUrl + authorId + '/posts/' + postId + '/comments';
        return this.http.get<Comment[]>(url).pipe(
            tap(data => console.log(data)),
            catchError(this.handleError)
        ) as Observable<Comment[]>;
    }

    postComment(authorId: string, postId: string, contents: string) {
        const url = this.authorsUrl + authorId + '/posts/' + postId + '/comments';
        const body = {
            contents: contents
        };

        return this.http.post<Comment>(url, body).pipe(
            tap(data => console.log(data)),
            catchError(this.handleError)
        ) as Observable<Comment>;
    }

    handleError(handleError: any): any{
        console.log(handleError);
    }
}