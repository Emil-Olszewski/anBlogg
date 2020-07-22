import { AuthorShort } from '../authors/author';

export interface Comment {
    id: number;
    author: AuthorShort;
    createad: Date;
    contents: string;
    score: number;
}

export class CommentsInfo {
    authorId: string;
    postId: string;
    amount: number;
}