import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constants } from 'src/app/constants';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { Author, AuthorCreateViewModel } from './author';
import { catchError, tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthorService {
    private authorUrl = Constants.API_URL + "authors/";

    private authorLoggedSubject = new BehaviorSubject<Author>(null);
    authorLoggedAction$ = this.authorLoggedSubject.asObservable();

    constructor(private http: HttpClient) { }

    getAuthor(id: string) {
        return this.http.get<Author>(this.authorUrl + id).pipe(
            tap(data => console.log(JSON.stringify(data))),
            catchError(this.handleError)
        ) as Observable<Author>
    };

    createAuthor(model: AuthorCreateViewModel) {
        return this.http.post<Author>(this.authorUrl, model).pipe(
            tap(data => console.log(JSON.stringify(data))),
            catchError(this.handleError)
        ) as Observable<Author>
    };

    handleError(error: any): any {
        console.log(error);
    }

    relateUserToAuthor(author: AuthorCreateViewModel) {
        this.getAuthor(author.id).subscribe(author => {
            if (!author) {
                this.createAuthor(author).subscribe(createdAuthor =>
                    this.setLoggedAuthor(createdAuthor));
            } else {
                this.setLoggedAuthor(author);
            }
        });
    }

    setLoggedAuthor(author: Author) {
        this.authorLoggedSubject.next(author);
    }

    get loggedAuthorId() {
        return this.authorLoggedSubject.getValue().id;
    }
}