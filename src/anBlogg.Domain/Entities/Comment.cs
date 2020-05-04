using anBlogg.Domain.Common;
using anBlogg.Domain.ValueObjects;
using System;

namespace anBlogg.Domain.Entities
{
    public class Comment : AuditableEntity, IScoreable
    {
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string Contents { get; set; }
        public Score Score { get; set; }
    }
}
