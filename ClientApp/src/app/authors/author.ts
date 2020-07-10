export interface AuthorShort {
    id: string;
    displayName: string;
    score: number;
}

export interface Author {
    id: string;
    displayName: string;
    score: number;
    postsNumber: number;
    commentsNumber: number;
}

export interface AuthorResolved {
    author: Author;
    error?: any;
}