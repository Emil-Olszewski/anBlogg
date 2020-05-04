using anBlogg.Domain.Common;
using anBlogg.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace anBlogg.Domain.Entities
{
    public class Author : AuditableEntity, IScoreable
    {
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public IList<Post> Posts { get; set; }
        public IList<Comment> Comments { get; set; }
        public Score Score { get; set; }

        public Author()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }
    }
}
