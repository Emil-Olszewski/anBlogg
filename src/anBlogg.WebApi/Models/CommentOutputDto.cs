using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace anBlogg.WebApi.Models
{
    public class CommentOutputDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
        public string Contents { get; set; }
        public int Score { get; set; }
    }
}
