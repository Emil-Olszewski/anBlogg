import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'comment-list',
    templateUrl: 'comment-list.component.html'
})

export class CommentListComponent implements OnInit {
    constructor() { }

    @Input()
    comments: Comment[];

    loaded: boolean;

    ngOnInit() { 
        console.log(this.comments);
    }
}