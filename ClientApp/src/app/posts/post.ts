import { AuthorShort } from '../authors/author';

export class Post {
    id: string;
    author: AuthorShort;
    created: Date;
    modified: Date;
    title: string;
    contents: string;
    tags: string[];
    score: number;
    commentsNumber: number;
}

export interface PostResolved {
    post: Post;
    error?: any;
}

export interface PostsResolved {
    posts: Post[],
    error?: any
}