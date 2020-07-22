using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using System;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IBlogRepository
    {
        PagedList<Author> GetAllAuthors(IResourceParameters parameters);

        Author GetAuthor(Guid id);

        bool AuthorNotExist(Guid id);

        PagedList<Post> GetPosts(IPostResourceParameters parameters);

        PagedList<Post> GetPostsForAuthor(Guid authorId, IPostResourceParameters parameters);

        Post GetPostForAuthor(Guid authorId, Guid postId);

        void AddPostForAuthor(Guid authorId, Post post);

        void DeletePost(Post post);

        int GetPostsNumberForAuthor(Guid id);

        bool PostNotExistForAuthor(Guid authorId, Guid postId);

        IEnumerable<Comment> GetCommentsForAuthor(Guid id);

        PagedList<Comment> GetAllComentsForPost(Guid postId, IResourceParameters parameters);

        Comment GetCommentForPost(Guid postId, Guid commentId);

        void AddCommentForPost(Guid postId, Comment comment);

        void DeleteComment(Comment comment);

        int GetCommentsNumberForAuthor(Guid id);

        int GetCommentsNumberForPost(Guid id);

        void SaveChanges();
        void AddAuthor(Author author);
    }
}