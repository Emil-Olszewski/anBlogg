using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.WebApi.Helpers;
using anBlogg.WebApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.WebApi.Controllers.Common
{
    public abstract class PostsControllerBase : CustomControllerBase
    {
        public PostsControllerBase(IMapper mapper, IBlogRepository blogRepository,
           IValidator validator, IProperties properties, IPagination pagination)
           : base(mapper, blogRepository, validator, properties, pagination)
        {
        }

        protected void InsertAuthorsInto(params Post[] posts)
        {
            foreach (var post in posts)
                post.Author = blogRepository.GetAuthor(post.AuthorId);
        }

        protected void InsertCommentsNumberInto(params PostOutputDto[] posts)
        {
            foreach (var post in posts)
                post.CommentsNumber = blogRepository.GetCommentsNumberForPost(post.Id);
        }

        protected override IEnumerable<LinkDto> CreateLinksForSingleResource(IIdsSet rawIds, string fields)
        {
            var ids = rawIds as PostIdsSet;
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link("GetPost",
                    new { ids.authorId, ids.postId }), "self", "GET"));
            else
                links.Add(new LinkDto(Url.Link("GetPost",
                new { ids.authorId, ids.postId, fields }), "self", "GET"));

            links.Add(new LinkDto(Url.Link("CreatePost",
                new { ids.authorId }), "create_post", "POST"));

            links.Add(new LinkDto(Url.Link("UpdatePost",
                     new { ids.authorId, ids.postId }), "update_post", "PUT"));

            links.Add(new LinkDto(Url.Link("GetPost",
                    new { ids.authorId, ids.postId }), "patch_post", "PATCH"));

            links.Add(new LinkDto(Url.Link("GetPost",
                    new { ids.authorId, ids.postId }), "detele_post", "DELETE"));

            return links;
        }

        protected override IEnumerable<IIdsSet> GetIds<T>(PagedList<T> resources)
        {
            var posts = resources as PagedList<Post>;
            return posts.Select(post =>
                new PostIdsSet(post.AuthorId, post.Id));
        }
    }
}