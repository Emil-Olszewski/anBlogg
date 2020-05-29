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

        public IEnumerable<Post> GetPosts(IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts;
            return DevelopQuery(query, parameters);
        }

        public IEnumerable<Post> GetAllPostsForAuthor(Guid authorId, IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts.Where(p => p.AuthorId == authorId);
            return DevelopQuery(query, parameters).ToList();
        }

        private IQueryable<Post> DevelopQuery(IQueryable<Post> query, IPostResourceParameters parameters)
        {
            if (parameters.RequiredTags != null)
                query = ApplyTags(query, parameters.RequiredTags);

            query = Paginate(query, parameters.PageNumber, parameters.PostsDisplayed);
            query = IncludeAuthorsAndComments(query);

            return query.AsNoTracking();
        }

        private IQueryable<Post> ApplyTags(IQueryable<Post> query, string requiredTags)
        {
            var enumeratedTags = tagsInString.EnumerateWithBrackets(requiredTags);
            foreach (var tag in enumeratedTags)
            {
                string likePhrase = $"%{tag}%";
                query = query.Where(p => EF.Functions.Like(p.Tags.Raw, likePhrase));
            }

            return query;
        }

        private IQueryable<Post> Paginate(IQueryable<Post> query, int pageNumber, int postsDisplayed)
        {
           return query.OrderByDescending(p => p.Created)
                   .Skip((pageNumber - 1) * postsDisplayed).Take(postsDisplayed);
        }

        private IQueryable<Post> IncludeAuthorsAndComments(IQueryable<Post> query)
        {
            query = query.Include(c => c.Author);
            return query.Include(c => c.Comments).ThenInclude(c => c.Author);
        }

        public void DeletePost(Post post)
        {
            context.Posts.Remove(post);
        }

        public Post GetPostForAuthor(Guid authorId, Guid postId)
        {
            return context.Posts
                .Where(p => p.AuthorId == authorId && p.Id == postId)
                .FirstOrDefault();
        }

        public void UpdatePostForAuthor(Guid authorId, Guid postId)
        {

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

        public IEnumerable<Comment> GetCommentsForAuthor(Guid id)
        {
            return context.Comments.Where(p => p.AuthorId == id).AsNoTracking();
        }

        public object GetAuthor(Guid id)
        {
            return context.Authors.Find(id);
        }

        public int GetPostsNumberForAuthor(Guid id)
        {
            return context.Posts.Where(p => p.AuthorId == id).Count();
        }

        public int GetCommentsNumberForAuthor(Guid id)
        {
            return context.Comments.Where(p => p.AuthorId == id).Count();
        }
    }
}
