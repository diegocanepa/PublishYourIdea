using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Business.Models
{
    public class EmailConfirmationTokenModelBussines
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Used { get; set; }
        public string Invalidated { get; set; }
        public int UserId { get; set; }
    }
}
