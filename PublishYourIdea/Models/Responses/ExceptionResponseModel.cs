using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Models.Responses
{
    public class ExceptionResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
