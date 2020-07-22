using anBlogg.Application.Services;
using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.Infrastructure.FluentValidation;
using anBlogg.Infrastructure.FluentValidation.Models;
using anBlogg.WebApi.Controllers.Common;
using anBlogg.WebApi.Helpers;
using anBlogg.WebApi.Models;
using anBlogg.WebApi.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace anBlogg.WebApi.Controllers
{
    [ApiController]
    [Route("api/authors/{postAuthorId}/posts/{postId}/comments")]
    public class CommentsController : CustomControllerBase
    {
        public CommentsController(IMapper mapper, IBlogRepository blogRepository,
           IValidator validator, IProperties properties, IPagination pagination)
           : base(mapper, blogRepository, validator, properties, pagination)
        { }

        [HttpOptions]
        public IActionResult GetCommentsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            return Ok();
        }

        [HttpGet(Name = "GetComments")]
        [HttpHead]
        public IActionResult GetCommentsForPost(Guid postAuthorId, Guid postId,
            [FromQuery] BasicResourceParameters parameters,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (CantValidateParameters(parameters))
                return BadRequest();

            if (validator.DontMatchRules(parameters, ModelState))
                return ValidationProblem(ModelState);

            if (AuthorOrPostNotExist(postAuthorId, postId))
                return NotFound();

            var commentsFromRepo = blogRepository.GetAllComentsForPost(postId, parameters);
            InsertAuthorsInto(commentsFromRepo.ToArray());

            var mappedComments = mapper.Map<IEnumerable<CommentOutputDto>>(commentsFromRepo);
            var shapedComments = properties.ShapeData(mappedComments, parameters.Fields);

            AddPaginationHeader(commentsFromRepo);

            if (IncludeLinks(mediaType))
            {
                var linkedComments = GetCollectionWithLinks
                    (commentsFromRepo, shapedComments, parameters);
                return Ok(linkedComments);
            }

            return Ok(shapedComments);
        }

        private bool CantValidateParameters(BasicResourceParameters parameters)
        {
            return validator.FieldsAreInvalid<CommentOutputDto>(parameters.Fields) ||
                validator.OrderIsInvalid<Comment, ICommentOutputDto>(parameters.OrderBy);
        }

        [HttpGet("{commentId}", Name = "GetComment")]
        public IActionResult GetComment(Guid postAuthorId, Guid postId, Guid commentId,
            string fields, [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (validator.FieldsAreInvalid<CommentOutputDto>(fields))
                return BadRequest();

            if (AuthorOrPostNotExist(postAuthorId, postId))
                return NotFound();

            var commentFromRepo = blogRepository.GetCommentForPost(postId, commentId);
            if (commentFromRepo is null)
                return NotFound();

            InsertAuthorsInto(commentFromRepo);

            var mappedComment = mapper.Map<CommentOutputDto>(commentFromRepo);
            var shapedComment = properties.ShapeSingleData(mappedComment, fields);

            if (IncludeLinks(mediaType))
            {
                var idsSet = new CommentIdsSet(postAuthorId, postId, commentId);
                var linkedResource = GetLinkedResource(shapedComment, idsSet, fields);
                return Ok(linkedResource);
            }

            return Ok(shapedComment);
        }

        [Authorize]
        [HttpPost(Name = "CreateComment")]
        public IActionResult CreateComment(Guid postAuthorId, Guid postId,
            CommentInputDto newComment, [FromHeader(Name = "Content-Type")] string mediaType)
        {
            var idsSet = new CommentIdsSet(postAuthorId, postId);
            newComment.AuthorId = GetUserId();
            return AddComment(idsSet, newComment, IncludeLinks(mediaType));
        }

        [HttpPut("{commentId}", Name = "UpdateComment")]
        public IActionResult UpdateComment(Guid postAuthorId, Guid postId, Guid commentId,
            CommentInputDto updatedComment, [FromHeader(Name = "Content-Type")] string mediaType)
        {
            var idsSet = new CommentIdsSet(postAuthorId, postId, commentId);
            var commentFromRepo = blogRepository.GetCommentForPost(postId, commentId);

            if (commentFromRepo is null)
                return AddComment(idsSet, updatedComment, IncludeLinks(mediaType));

            mapper.Map(updatedComment, commentFromRepo);
            blogRepository.SaveChanges();

            if (IncludeLinks(mediaType))
            {
                var mappedComment = mapper.Map<CommentOutputDto>(commentFromRepo);
                return Ok(ShapeAndLinkSingleComment(mappedComment, idsSet));
            }

            return NoContent();
        }

        private IActionResult AddComment(CommentIdsSet idsSet,
            CommentInputDto newComment, bool includeLinks = false)
        {
            if (validator.DontMatchRules(newComment as ICommentInputDto, ModelState))
                return ValidationProblem(ModelState);

            if (AuthorOrPostNotExist(idsSet.postAuthorId, idsSet.postId))
                return NotFound();

            var commentToAdd = mapper.Map<Comment>(newComment);
            InsertAuthorsInto(commentToAdd);

            if (idsSet.commentId != Guid.Empty)
                commentToAdd.Id = idsSet.commentId;

            blogRepository.AddCommentForPost(idsSet.postId, commentToAdd);
            blogRepository.SaveChanges();

            var mappedComment = mapper.Map<CommentOutputDto>(commentToAdd);
            idsSet.commentId = mappedComment.Id;

            dynamic toReturn = mappedComment;

            if (includeLinks)
                toReturn = ShapeAndLinkSingleComment(mappedComment, idsSet);

            return CreatedAtRoute("GetComment",
                new { idsSet.postAuthorId, idsSet.postId, commentId = mappedComment.Id }, toReturn);
        }

        private void InsertAuthorsInto(params Comment[] comments)
        {
            foreach (var comment in comments)
                comment.Author = blogRepository.GetAuthor(comment.AuthorId);
        }

        [Authorize]
        [HttpPatch("{commentId}", Name = "PatchComment")]
        public IActionResult PartiallyUpdateComment(Guid postAuthorId, Guid postId, Guid commentId,
            JsonPatchDocument<CommentInputDto> patchDocument,
            [FromHeader(Name = "Content-Type")] string mediaType)
        {
            if (AuthorOrPostNotExist(postAuthorId, postId))
                return NotFound();

            var commentFromRepo = blogRepository.GetCommentForPost(postId, commentId);
            if (commentFromRepo is null)
                return NotFound();

            var commentInputDto = mapper.Map<CommentInputDto>(commentFromRepo);
            patchDocument.ApplyTo(commentInputDto, ModelState);

            if (validator.DontMatchRules(commentInputDto as ICommentInputDto, ModelState))
                return ValidationProblem(ModelState);

            mapper.Map(commentInputDto, commentFromRepo);
            blogRepository.SaveChanges();

            if (IncludeLinks(mediaType))
            {
                var idsSet = new CommentIdsSet(postAuthorId, postId, commentFromRepo.Id);
                var mappedComment = mapper.Map<CommentOutputDto>(commentFromRepo);
                return Ok(ShapeAndLinkSingleComment(mappedComment, idsSet));
            }

            return NoContent();
        }

        private IDictionary<string, object> ShapeAndLinkSingleComment
            (CommentOutputDto commentToReturn, CommentIdsSet idsSet)
        {
            var shapedComment = properties.ShapeSingleData(commentToReturn);
            return GetLinkedResource(shapedComment, idsSet);
        }

        [Authorize]
        [HttpDelete("{commentId}", Name = "DeleteComment")]
        public IActionResult DeleteComment(Guid postAuthorId, Guid postId, Guid commentId)
        {
            if (AuthorOrPostNotExist(postAuthorId, postId))
                return NotFound();

            var commentFromRepo = blogRepository.GetCommentForPost(postId, commentId);
            if (commentFromRepo is null)
                return NotFound();

            blogRepository.DeleteComment(commentFromRepo);
            blogRepository.SaveChanges();

            return NoContent();
        }

        private bool AuthorOrPostNotExist(Guid postAuthorId, Guid postId)
        {
            return blogRepository.AuthorNotExist(postAuthorId) ||
                blogRepository.PostNotExistForAuthor(postAuthorId, postId);
        }

        protected override IEnumerable<LinkDto> CreateLinksForSingleResource
            (IIdsSet rawIds, string fields)
        {
            var ids = rawIds as CommentIdsSet;
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link("GetComment",
                    new { ids.postAuthorId, ids.postId, ids.commentId }), "self", "GET"));
            else
                links.Add(new LinkDto(Url.Link("GetComment",
                    new { ids.postAuthorId, ids.postId, ids.commentId, fields }), "self", "GET"));

            links.Add(new LinkDto(Url.Link("CreateComment",
                 new { ids.postAuthorId, ids.postId }), "create_comment", "POST"));

            links.Add(new LinkDto(Url.Link("UpdateComment",
                new { ids.postAuthorId, ids.postId, ids.commentId }), "update_comment", "PUT"));

            links.Add(new LinkDto(Url.Link("PatchComment",
                new { ids.postAuthorId, ids.postId, ids.commentId }), "patch_comment", "PATCH"));

            links.Add(new LinkDto(Url.Link("DeleteComment",
                new { ids.postAuthorId, ids.postId, ids.commentId }), "delete_comment", "DELETE"));

            return links;
        }

        protected override IEnumerable<IIdsSet> GetIds<T>(PagedList<T> resources)
        {
            var comments = resources as PagedList<Comment>;
            return comments.Select(comment =>
                new CommentIdsSet(comment.AuthorId, comment.PostId, comment.Id));
        }
    }
}