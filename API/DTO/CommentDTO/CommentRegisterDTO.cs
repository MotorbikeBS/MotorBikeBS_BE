using System;
using System.Collections.Generic;

namespace API.DTO.CommentDTO
{
    public partial class CommentRegisterDTO
    {
        public string? Content { get; set; }
        public int? Rating { get; set; }
    }
}
