﻿using System.Collections.Generic;
using System.Net;

namespace API.DTO
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
