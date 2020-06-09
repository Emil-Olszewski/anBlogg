using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.Application.Services.Implementations
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IBlogContext context;
        private readonly ITagsInString tagsInString;
        private readonly IPropertyMappingService mappingService;
        private readonly IQueryableSorter queryableSorter;

        public BlogRepository(IBlogContext context, ITagsInString tagsInString,
            IPropertyMappingService mappingService, IQueryableSorter queryableSorter)
        {
            this.context = context;
            this.tagsInString = tagsInString;
            this.mappingService = mappingService;
            this.queryableSorter = queryableSorter;
        }

        public PagedList<Author> GetAllAuthors(IResourceParameters parameters)
        {
            var authors = context.Authors.AsNoTracking();
            return PagedList<Author>.Create(authors, parameters.PageNumber, parameters.PageSize);
        }

        public Author GetAuthor(Guid id) =>
            context.Authors.Find(id);

        public PagedList<Post> GetPosts(IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts;
            return DevelopPostsQuery(query, parameters);
        }

        public PagedList<Post> GetPostsForAuthor
            (Guid authorId, IPostResourceParameters parameters)
        {
            IQueryable<Post> query = context.Posts.Where(p => p.AuthorId == authorId);
            return DevelopPostsQuery(query, parameters);
        }

        private PagedList<Post> DevelopPostsQuery
            (IQueryable<Post> query, IPostResourceParameters parameters)
        {
            if (parameters.Tags != null)
                query = ApplyTags(query, parameters.Tags);

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
                query = ApplySorting(query, parameters.OrderBy);

            query = IncludeAuthorsAndComments(query);
            query = query.AsNoTracking();

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
            return queryableSorter.ApplySort(query, orderBy, postMappingDictionary);
        }

        public Post GetPostForAuthor(Guid authorId, Guid postId)
        {
            var query = context.Posts.Where(p => p.AuthorId == authorId && p.Id == postId);
            query = IncludeAuthorsAndComments(query);
            return query.FirstOrDefault();
        }

        private IQueryable<Post> IncludeAuthorsAndComments(IQueryable<Post> query)
        {
            query = query.Include(c => c.Author);
            query = query.Include(c => c.Comments).ThenInclude(c => c.Author);
            return query;
        }

        public void AddPostForAuthor(Guid authorId, Post post)
        {
            post.AuthorId = authorId;
            context.Posts.Add(post);
        }

        public void UpdatePostForAuthor(Guid authorId, Guid postId)
        {
        }

        public void DeletePost(Post post) =>
            context.Posts.Remove(post);

        public IEnumerable<Comment> GetCommentsForAuthor(Guid id) =>
            context.Comments.Where(p => p.AuthorId == id).AsNoTracking();

        public int GetPostsNumberForAuthor(Guid id) =>
            context.Posts.Where(p => p.AuthorId == id).Count();

        public int GetCommentsNumberForAuthor(Guid id) =>
            context.Comments.Where(p => p.AuthorId == id).Count();

        public bool AuthorNotExist(Guid id) =>
            !context.Authors.Any(a => a.Id == id);

        public void SaveChanges() =>
            context.SaveChanges();
    }
}