using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using System;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IBlogRepository
    {
        IEnumerable<Post> GetAllPosts(IPostResourceParameters parameters);
        IEnumerable<Post> GetAllPostsForAuthor(Guid authorId, IPostResourceParameters parameters);
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
    }
}