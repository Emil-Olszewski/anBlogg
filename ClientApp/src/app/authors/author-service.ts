import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constants } from 'src/app/constants';
import { BehaviorSubject, Observable } from 'rxjs';
import { Author } from './author';
import { catchError, tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthorService {
    private authorUrl = Constants.API_URL + "authors/";

    constructor(private http: HttpClient) { }

    getAuthor(id: string) {
        return this.http.get<Author>(this.authorUrl + id).pipe(
            tap(data => console.log(JSON.stringify(data))),
            catchError(this.handleError)
        ) as Observable<Author>
    }

    handleError(error: any): any {
        console.log(error);
    }

}