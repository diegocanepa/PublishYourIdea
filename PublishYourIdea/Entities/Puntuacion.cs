using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.Entities
{
    public partial class Puntuacion
    {
        public int IdPuntuacion { get; set; }
        public int IdIdea { get; set; }
        public int IdUsuario { get; set; }
        public int Estrellas { get; set; }

        public virtual Idea IdIdeaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
