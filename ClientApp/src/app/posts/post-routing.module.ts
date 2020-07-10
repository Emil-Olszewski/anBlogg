import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PostsResolver } from './posts-resolver';
import { PostUrlListComponent } from './post-url-list/post-url-list.component';
import { PostEditComponent } from './post-edit/post-edit.component';
import { PostResolver } from './post-resolver';

const routes: Routes = [
  {
    path: 'posts',
    children: [
      {
        path: 'edit/:id',
        component: PostEditComponent,
        resolve: { resolvedData: PostResolver },
      },
      {
        path: 'edit',
        redirectTo: 'edit/0',
        pathMatch: 'full',
      },
      {
        path: ':page',
        component: PostUrlListComponent,
        resolve: { resolvedData: PostsResolver },
      },
      {
        path: '',
        redirectTo: '1',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PostRoutingModule {}
