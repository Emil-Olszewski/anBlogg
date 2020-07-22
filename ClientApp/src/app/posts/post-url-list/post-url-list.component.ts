import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Pagination } from 'src/app/page-navigation/pagination';

@Component({
    selector: 'selector-name',
    templateUrl: 'post-url-list.component.html'
})

export class PostUrlListComponent implements OnInit {
    posts$;
    errorMessage;
    paginationInfo: Pagination;

    constructor(private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.route.data.subscribe(data => {
            this.paginationInfo = JSON.parse(data['resolvedData'].posts.headers.get("x-pagination"));
            this.posts$ = data['resolvedData'].posts.body;
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