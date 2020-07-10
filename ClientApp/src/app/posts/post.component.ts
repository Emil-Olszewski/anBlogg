import { Component, OnInit, Input } from '@angular/core';
import { Post } from './post';
import { Constants } from '../constants';

@Component({
    selector: 'post',
    templateUrl: 'post.component.html'
})

export class PostComponent implements OnInit {
    @Input()
    post: Post;

    postContents: string;
    postContentsShortened: string;
    longEnoughToHide: boolean;
    hided: boolean;

    constructor() { }

    ngOnInit() {
        console.log(this.post);
        if (this.post.contents.length > Constants.POST_SHORTENED_LENGTH) {
            this.hided = this.longEnoughToHide = true;
            this.postContentsShortened =
                this.post.contents.slice(0, Constants.POST_SHORTENED_LENGTH) + ' [...]';
            this.postContents = this.postContentsShortened;
        } else {
            this.postContents = this.post.contents;
        }
    }

    onExpand() {
        this.postContents = this.post.contents;
        this.hided = false;
    }

    onFold() {
        this.postContents = this.postContentsShortened;
        this.hided = true;
    }
}