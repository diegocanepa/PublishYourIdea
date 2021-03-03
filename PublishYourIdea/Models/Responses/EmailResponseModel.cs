using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Models.Responses
{
    public class EmailResponseModel
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
