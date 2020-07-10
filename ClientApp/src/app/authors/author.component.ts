import { Component, OnInit } from '@angular/core';
import { AuthorService } from './author-service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'post-author',
    templateUrl: 'author.component.html'
})

export class AuthorComponent implements OnInit {
    author;
    errorMessage;

    constructor(private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.data.subscribe(data => {
            this.author = data['resolvedData'].author;
            this.errorMessage = data['resolvedData'].error;
        });
    }

}