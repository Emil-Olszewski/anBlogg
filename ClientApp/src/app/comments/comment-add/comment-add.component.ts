import { Component, OnInit, Input } from '@angular/core';
import { Constants } from 'src/app/constants';
import { CommentsInfo } from '../comment';
import { CommentService } from '../comment.service';
import { AuthService } from 'src/app/core/auth-service.component';

@Component({
    selector: 'comment-add',
    templateUrl: 'comment-add.component.html'
})

export class CommentAddComponent implements OnInit {
    @Input()
    commentsInfo: CommentsInfo;
    @Input()
    comments: Comment[];
    comment: string;
    commentMax = Constants.COMMENT_MAX_LENGTH;
    isLoggedIn = false;

    constructor(private commentService: CommentService, private authService: AuthService) { }

    ngOnInit() {
        this.authService.isLoggedIn().then(value => this.isLoggedIn = value);
    }

    onSend(comment: string) {
        if (this.isLoggedIn) {
            this.commentService
                .postComment(this.commentsInfo.authorId, this.commentsInfo.postId, comment)
                .subscribe(newComment => {
                    this.comment = '';
                    this.comments.push(newComment);
                });
        }
    }
}