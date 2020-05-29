using anBlogg.Domain.Common;
using anBlogg.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace anBlogg.Domain.Entities
{
    public class Post : AuditableEntity, IScoreable
    {
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public Tags Tags { get; set; }
        public Score Score { get; set; }
        public IList<Comment> Comments { get; set; }
            =  new List<Comment>();
    }
}
