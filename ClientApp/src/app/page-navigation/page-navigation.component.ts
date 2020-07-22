import { Component, OnInit, Input } from '@angular/core';
import { Pagination } from './pagination';

@Component({
    selector: 'page-navigation',
    templateUrl: 'page-navigation.component.html'
})

export class PageNavigationComponent implements OnInit {
    @Input()
    pagination: Pagination


    constructor() { }

    ngOnInit() {
        console.log(this.pagination);
     }

    get pageNumber() {
        return this.pagination.CurrentPage;
    }

    nextPageExists() {
        return this.pagePlusExists(1);
    }

    previousPageExists() { 
        return this.pageMinusExists(1);
    }

    pagePlusExists(num: number) {
        return this.pagination.TotalPages >= this.pagination.CurrentPage + num;
    }

    pageMinusExists(num: number) {
        return this.pagination.CurrentPage - num > 0;
    }
}