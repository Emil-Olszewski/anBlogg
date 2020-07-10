import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/constants';

@Component({
    selector: 'comment-add',
    templateUrl: 'comment-add.component.html'
})

export class CommentAddComponent implements OnInit {
    comment: string;
    commentMax = Constants.COMMENT_MAX_LENGTH;
    constructor() { }

    ngOnInit() { }

    onSend(comment: string) { }
}