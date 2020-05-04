using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Domain.Services;
using anBlogg.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.Application.Services.Implementations
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext context;
        private readonly ITagsInString tagsInString;

        public BlogRepository(BlogContext context, ITagsInString tagsInString)
        {
            this.context = context;
            this.tagsInString = tagsInString;
        }

        public IEnumerable<Post> GetAllPosts(IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts;
            return DevelopQuery(query, parameters);
        }

        public IEnumerable<Post> GetAllPostsForAuthor(Guid authorId, IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts.Where(p => p.AuthorId == authorId);
            return DevelopQuery(query, parameters);
        }

        private IEnumerable<Post> DevelopQuery(IQueryable<Post> query, IPostResourceParameters parameters)
        {
            if (parameters != null && parameters.RequiredTags != null)
            {
                var requiredTags = tagsInString.EnumerateWithBrackets(parameters.RequiredTags);
                foreach (var tag in requiredTags)
                {
                    string likePhrase = $"%{tag}%";
                    query = query.Where(p => EF.Functions.Like(p.Tags.Raw, likePhrase));
                }
            }

            return query.AsNoTracking();
        }

        public void DeletePost(Post post)
        {
            context.Posts.Remove(post);
        }

        public Post GetPostForAuthor(Guid authorId, Guid postId)
        {
            return context.Posts.Where(p => p.AuthorId == authorId && p.Id == postId)
                .AsNoTracking().FirstOrDefault();
        }

        public void UpdatePostForAuthor(Guid authorId, Guid postId)
        {
            throw new NotImplementedException();
        }

        public void AddPostForAuthor(Guid authorId, Post post)
        {
            post.AuthorId = authorId;
            context.Posts.Add(post);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public bool AuthorNotExist(Guid id)
        {
            return !context.Authors.Any(a => a.Id == id);
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return context.Authors.AsNoTracking();
        }

        public int GetNumberOfCommentsForAuthor(Guid id)
        {
            return context.Posts.Where(p => p.AuthorId == id).Count();
        }

        public int GetNumberOfPostsForAuthor(Guid id)
        {
            return context.Comments.Where(p => p.AuthorId == id).Count();
        }

        public IEnumerable<Comment> GetCommentsForAuthor(Guid id)
        {
            return context.Comments.Where(p => p.AuthorId == id).AsNoTracking();
        }
    }
}
