using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class EmailNotificacion
    {
        public string IdEmailNotificacion { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public byte[] Contenido { get; set; }

        public virtual Usuario User { get; set; }
    }
}
