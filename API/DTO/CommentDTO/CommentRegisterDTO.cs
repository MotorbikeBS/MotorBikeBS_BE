using System;
using System.Collections.Generic;

namespace API.DTO.CommentDTO
{
    public partial class CommentRegisterDTO
    {
        public int RequestId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public string? Status { get; set; }
        public int? ReplyId { get; set; }
    }
}
