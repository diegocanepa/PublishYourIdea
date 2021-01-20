﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Business.Models
{
    public class AuthenticationResultModelBusiness
    {
        public string Token { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
