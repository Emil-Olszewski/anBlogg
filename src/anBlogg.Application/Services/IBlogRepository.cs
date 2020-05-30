using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using System;
using System.Collections.Generic;

namespace anBlogg.Application.Services
{
    public interface IBlogRepository
    {
        PagedList<Post> GetPosts(IPostResourceParameters parameters);

        PagedList<Post> GetPostsForAuthor(Guid authorId, IPostResourceParameters parameters);

        Post GetPostForAuthor(Guid authorId, Guid postId);

        void AddPostForAuthor(Guid authorId, Post post);

        void UpdatePostForAuthor(Guid authorId, Guid postId);

        void DeletePost(Post post);

        IEnumerable<Author> GetAllAuthors();

        IEnumerable<Comment> GetCommentsForAuthor(Guid authorId);

        bool AuthorNotExist(Guid id);

        Author GetAuthor(Guid id);

        int GetPostsNumberForAuthor(Guid id);

        int GetCommentsNumberForAuthor(Guid id);
        void SaveChanges();
    }
}