
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CommentListComponent } from './comment-list/comment-list.component';
import { CommentAddComponent } from './comment-add/comment-add.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule
    ],
    exports: [CommentListComponent],
    declarations: [
        CommentListComponent,
        CommentAddComponent
    ],
    providers: [],
})
export class CommentModule { }
