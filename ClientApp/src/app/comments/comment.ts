import { AuthorShort } from '../authors/author-short';


export interface Comment {
    id: number;
    author: AuthorShort;
    createad: Date;
    contents: string;
    score: number;
}