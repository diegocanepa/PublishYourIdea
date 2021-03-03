﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Business.Models
{
    public class AuthenticationResultModelBusiness
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string message { get; set; }
        public UsuarioModelBusiness user { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
