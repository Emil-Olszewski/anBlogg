using System;

namespace anBlogg.WebApi.Helpers
{
    public interface IIdsSet
    {
    }

    #region Author
    public class AuthorIdsSet : IIdsSet
    {
        public Guid authorId;

        public AuthorIdsSet(Guid authorId)
        {
            this.authorId = authorId;
        }
    }
    #endregion

    #region Post
    public class PostIdsSet : IIdsSet
    {
        public Guid authorId;
        public Guid postId;

        public PostIdsSet(Guid authorId)
        {
            this.authorId = authorId;
            this.postId = Guid.Empty;
        }

        public PostIdsSet(Guid authorId, Guid postId)
        {
            this.authorId = authorId;
            this.postId = postId;
        }
    }
    #endregion

    #region Comment
    public class CommentIdsSet : IIdsSet
    {
        public Guid postAuthorId;
        public Guid postId;
        public Guid commentId;
        public Guid commentAuthorId;

        public CommentIdsSet(Guid postAuthorId, Guid postId)
        {
            this.postAuthorId = postAuthorId;
            this.postId = postId;
            commentId = Guid.Empty;
            commentAuthorId = Guid.Empty;
        }

        public CommentIdsSet(Guid postAuthorId, Guid postId, Guid commentId)
        {
            this.postAuthorId = postAuthorId;
            this.postId = postId;
            this.commentId = commentId;
            commentAuthorId = Guid.Empty;
        }
    }
    #endregion
}