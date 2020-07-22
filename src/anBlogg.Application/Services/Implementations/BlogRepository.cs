using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

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

        public Author GetAuthor(Guid id)
        {
            return context.Authors.Find(id);
        }

        public void AddAuthor(Author author)
        {
            context.Authors.Add(author);
        }

        public bool AuthorNotExist(Guid id)
        {
            return !context.Authors.Any(a => a.Id == id);
        }

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
            query = query.AsNoTracking();

            if (parameters.Tags != null)
                query = ExcludePostWithoutTags(query, parameters.Tags);

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
                query = SortPosts(query, parameters.OrderBy);

            query = query.Include(post => post.Author);

            return PagedList<Post>.Create(query, parameters.PageNumber, parameters.PageSize);
        }

        private IQueryable<Post> ExcludePostWithoutTags
            (IQueryable<Post> query, string requiredTags)
        {
            var enumeratedTags = tagsInString.EnumerateWithBrackets(requiredTags);
            foreach (var tag in enumeratedTags)
            {
                string likePhrase = $"%{tag}%";
                query = query.Where(p => EF.Functions.Like(p.Tags.Raw, likePhrase));
            }

            return query;
        }

        private IQueryable<Post> SortPosts(IQueryable<Post> query, string orderBy)
        {
            var postMappingDictionary = mappingService.GetPropertyMapping<IPostOutputDto, Post>();
            return queryableSorter.ApplySort(query, orderBy, postMappingDictionary);
        }

        public Post GetPostForAuthor(Guid authorId, Guid postId)
        {
            var query = context.Posts
                .Where(post => post.AuthorId == authorId && post.Id == postId);

            query = query
                .Include(c => c.Author);

            query = query
                .Include(c => c.Comments)
                .ThenInclude(c => c.Author);

            return query.FirstOrDefault();
        }

        public void AddPostForAuthor(Guid authorId, Post post)
        {
            post.AuthorId = authorId;
            context.Posts.Add(post);
        }

        public void DeletePost(Post post)
        {
            context.Posts.Remove(post);
        }

        public int GetPostsNumberForAuthor(Guid id)
        {
            return context.Posts
                .Where(p => p.AuthorId == id)
                .Count();
        }

        public bool PostNotExistForAuthor(Guid authorId, Guid postId)
        {
            return !context.Posts
                .Any(post => authorId == post.AuthorId && postId == post.Id);
        }

        public PagedList<Comment> GetAllComentsForPost
            (Guid postId, IResourceParameters parameters)
        {
            var comments = context.Comments
                .Where(comment => postId == comment.PostId).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
                comments = SortComments(comments, parameters.OrderBy);

            return PagedList<Comment>.Create
                (comments, parameters.PageNumber, parameters.PageSize);
        }

        private IQueryable<Comment> SortComments(IQueryable<Comment> comments, string orderBy)
        {
            var commentMappingDictionary =
                mappingService.GetPropertyMapping<ICommentOutputDto, Comment>();

            return queryableSorter.ApplySort
                (comments, orderBy, commentMappingDictionary);
        }

        public Comment GetCommentForPost(Guid postId, Guid commentId)
        {
            return context.Comments
                .Where(comment => comment.PostId == postId && comment.Id == commentId)
                .FirstOrDefault();
        }

        public IEnumerable<Comment> GetCommentsForAuthor(Guid id)
        {
            return context.Comments
                .Where(p => p.AuthorId == id)
                .AsNoTracking();
        }

        public void AddCommentForPost(Guid postId, Comment comment)
        {
            comment.PostId = postId;
            context.Comments.Add(comment);
        }

        public void DeleteComment(Comment comment)
        {
            context.Comments.Remove(comment);
        }

        public int GetCommentsNumberForPost(Guid id)
        {
            return context.Comments
                .Where(comment => comment.PostId == id)
                .Count();
        }

        public int GetCommentsNumberForAuthor(Guid id)
        {
            return context.Comments
                .Where(p => p.AuthorId == id)
                .Count();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}