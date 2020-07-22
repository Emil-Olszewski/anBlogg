import { Component, OnInit, Input } from '@angular/core';
import { CommentsInfo } from '../comment';
import { CommentService } from '../comment.service';

@Component({
    selector: 'comment-list',
    templateUrl: 'comment-list.component.html'
})

export class CommentListComponent {
    @Input()
    commentsInfo: CommentsInfo;
    comments: Comment[];
    loaded: boolean;
    visible: boolean;

    constructor(private commentService: CommentService) { }

    loadComments() {
        if (!this.loaded) {
            this.commentService.getComments(this.commentsInfo.authorId, this.commentsInfo.postId)
                .subscribe(data => {
                    this.comments = data;
                    this.loaded = this.visible = true;                  
                });
        }
    }
}