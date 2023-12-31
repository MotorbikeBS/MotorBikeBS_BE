﻿using API.DTO.RequestDTO;
using Core.Models;
using System;
using System.Collections.Generic;

namespace API.DTO.CommentDTO
{
    public partial class CommentResponseDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Status { get; set; }
        public int RequestId { get; set; }
        public virtual Store_RequestResponseDTO Request { get; set; } = null!;
        public int? ReplyId { get; set; }
        public virtual ICollection<ReplyCommentResponseDTO> InverseReply { get; set; }
    }
}
