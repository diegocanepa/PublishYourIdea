using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Models.Responses
{
    public class AuthResponseModel
    {
        public bool success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UsuarioModel user { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}
