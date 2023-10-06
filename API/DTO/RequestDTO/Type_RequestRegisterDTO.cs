using System;
using System.Collections.Generic;
using Core.Models;

namespace API.DTO.RequestDTO
{
    public partial class Type_RequestRegisterDTO
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

    }
}
