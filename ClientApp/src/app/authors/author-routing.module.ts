import { NgModule } from '@angular/core';

import { Routes, RouterModule } from '@angular/router';
import { AuthorComponent } from './author.component';
import { AuthorResolver } from './author-resolver';
import { PostUrlListComponent } from '../posts/post-url-list/post-url-list.component';
import { AuthorPostsResolver } from './author-posts-resolver';
import { PostsResolver } from '../posts/posts-resolver';

export const routes: Routes = [
    {
        path: 'authors', children:
            [
                {
                    path: ':id',
                    component: AuthorComponent,
                    resolve: { resolvedData: AuthorResolver },
                    children: [
                        {
                            path: 'posts/:page',
                            component: PostUrlListComponent,
                            resolve: { resolvedData: PostsResolver }
                        }
                    ]
                },
            ]
    },

]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AuthorRoutingModule { }
