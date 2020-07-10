import { NgModule } from '@angular/core';

import { AuthorComponent } from './author.component';
import { AuthorRoutingModule } from './author-routing.module';
import { CommonModule } from '@angular/common';
import { PostsModule } from '../posts/post.module';

@NgModule({
    imports: [PostsModule, AuthorRoutingModule, CommonModule],
    exports: [],
    declarations: [AuthorComponent],
    providers: [],
})
export class AuthorModule { }
