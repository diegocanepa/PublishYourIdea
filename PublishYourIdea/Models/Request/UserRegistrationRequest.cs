using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Models.Request
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [Password]
        public string Password { get; set; }
    }
}
