using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using System;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IBlogRepository
    {
        IEnumerable<Post> GetPosts(IPostResourceParameters parameters);
        IEnumerable<Post> GetAllPostsForAuthor(Guid authorId, IPostResourceParameters parameters);
        IEnumerable<Author> GetAuthors(Guid[] ids);
        Post GetPostForAuthor(Guid authorId, Guid postId);
        void AddPostForAuthor(Guid authorId, Post post);
        void UpdatePostForAuthor(Guid authorId, Guid postId);
        void DeletePost(Post post);
        IEnumerable<Author> GetAllAuthors();
        IEnumerable<Comment> GetCommentsForAuthor(Guid authorId);
        int GetNumberOfPostsForAuthor(Guid id);
        int GetNumberOfCommentsForAuthor(Guid id);
        bool AuthorNotExist(Guid id);
        void SaveChanges();
        int GetNumberOfCommentsForPost(Guid id);
    }
}