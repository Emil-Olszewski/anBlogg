using anBlogg.Application.Services.Helpers;
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
        private readonly IPropertyMappingService mappingService;

        public BlogRepository(BlogContext context, 
            ITagsInString tagsInString, IPropertyMappingService mappingService)
        {
            this.context = context;
            this.tagsInString = tagsInString;
            this.mappingService = mappingService;
        }

        public PagedList<Post> GetPosts(IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts;
            return DevelopQuery(query, parameters);
        }

        public PagedList<Post> GetPostsForAuthor(Guid authorId, 
            IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts.Where(p => p.AuthorId == authorId);
            return DevelopQuery(query, parameters);
        }

        private PagedList<Post> DevelopQuery(IQueryable<Post> query, 
            IPostResourceParameters parameters)
        {
            if (parameters.Tags != null)
                query = ApplyTags(query, parameters.Tags);

            if(!string.IsNullOrWhiteSpace(parameters.OrderBy))
                query = ApplySorting(query, parameters.OrderBy);

            query = IncludeAuthorsAndComments(query).AsNoTracking();
            return PagedList<Post>.Create(query, parameters.PageNumber, parameters.PageSize);
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

        private IQueryable<Post> ApplySorting(IQueryable<Post> query, string orderBy)
        {
            var postMappingDictionary = mappingService.GetPropertyMapping<IPostOutputDto, Post>();
            return query.ApplySort(orderBy, postMappingDictionary);
        }

        private IQueryable<Post> IncludeAuthorsAndComments(IQueryable<Post> query)
        {
            query = query.Include(c => c.Author);
            query = query.Include(c => c.Comments).ThenInclude(c => c.Author);
            return query;
        }

        public void DeletePost(Post post) => 
            context.Posts.Remove(post);

        public Post GetPostForAuthor(Guid authorId, Guid postId)
        {
            return context.Posts
                .Where(p => p.AuthorId == authorId && p.Id == postId)
                .FirstOrDefault();
        }

        public void UpdatePostForAuthor(Guid authorId, Guid postId) { }

        public void AddPostForAuthor(Guid authorId, Post post)
        {
            post.AuthorId = authorId;
            context.Posts.Add(post);
        }

        public bool AuthorNotExist(Guid id) => 
            !context.Authors.Any(a => a.Id == id);

        public IEnumerable<Author> GetAllAuthors()
        {
            IQueryable<Author> queryable = context.Authors.AsNoTracking();
            return queryable;
        }

        public IEnumerable<Comment> GetCommentsForAuthor(Guid id) => 
            context.Comments.Where(p => p.AuthorId == id).AsNoTracking();

        public Author GetAuthor(Guid id) => 
            context.Authors.Find(id);

        public int GetPostsNumberForAuthor(Guid id) => 
            context.Posts.Where(p => p.AuthorId == id).Count();

        public int GetCommentsNumberForAuthor(Guid id) => 
            context.Comments.Where(p => p.AuthorId == id).Count();

        public void SaveChanges() => 
            context.SaveChanges();
    }
}