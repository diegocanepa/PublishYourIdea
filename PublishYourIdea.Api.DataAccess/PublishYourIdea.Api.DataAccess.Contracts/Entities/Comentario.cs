using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class Comentario
    {
        public int IdComentario { get; set; }
        public string DescripcionComentario { get; set; }
        public int IdUsuario { get; set; }
        public int IdIdea { get; set; }
        public DateTime? FechaComentario { get; set; }
        public int? MeGustas { get; set; }
        public int? NoMeGustas { get; set; }

        public virtual Idea IdIdeaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
