using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseReply = new HashSet<Comment>();
        }

        public int CommentId { get; set; }
        public int RequestId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Status { get; set; }
        public int? ReplyId { get; set; }

        public virtual Comment? Reply { get; set; }
        public virtual Request Request { get; set; } = null!;
        public virtual ICollection<Comment> InverseReply { get; set; }
    }
}
