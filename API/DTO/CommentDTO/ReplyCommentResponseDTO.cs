using System;
using System.Collections.Generic;

namespace API.DTO.CommentDTO
{
    public partial class ReplyCommentResponseDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Status { get; set; }
    }
}
