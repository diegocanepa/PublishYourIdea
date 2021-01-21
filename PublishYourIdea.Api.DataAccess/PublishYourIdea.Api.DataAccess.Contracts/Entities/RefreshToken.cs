using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Used { get; set; }
        public string Invalidated { get; set; }
        public int? UserId { get; set; }

        public virtual Usuario User { get; set; }
    }
}
