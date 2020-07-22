import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostListComponent } from './post-list/post-list.component';
import { PostRoutingModule } from './post-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { CommentModule } from '../comments/comment.module';
import { PostUrlListComponent } from './post-url-list/post-url-list.component';
import { PostComponent } from './post.component';
import { PostEditComponent } from './post-edit/post-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PageNavigationComponent } from '../page-navigation/page-navigation.component';

@NgModule({
  declarations: [
    PostComponent,
    PostUrlListComponent,
    PostListComponent,
    PostEditComponent,
    PageNavigationComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CommentModule,
    PostRoutingModule,
    HttpClientModule,
  ],
  providers: [],
})
export class PostsModule {}
