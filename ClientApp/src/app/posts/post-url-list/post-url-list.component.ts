import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'selector-name',
    templateUrl: 'post-url-list.component.html'
})

export class PostUrlListComponent implements OnInit {
    posts$;
    errorMessage;

    constructor(private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.route.data.subscribe(data => {
            this.posts$ = data['resolvedData'].posts;
            this.errorMessage = data['resolvedData'].error;
        });
    }

    get pageNumber(): number {
        return +this.route.snapshot.paramMap.get('page');
    }

    onPostsDisplayedChanged(number: number) {
        this.router.navigate([], {
            relativeTo: this.route,
            queryParams: { postsDisplayed: number}
        });
    }
}