using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.Entities
{
    public partial class Idea
    {
        public Idea()
        {
            Comentario = new HashSet<Comentario>();
            MultimediaIdea = new HashSet<MultimediaIdea>();
            Puntuacion = new HashSet<Puntuacion>();
        }

        public int IdIdea { get; set; }
        public int NroIdea { get; set; }
        public int NroVersion { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int? Estrellas { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaBaja { get; set; }
        public string ComentarioBaja { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstado { get; set; }

        public virtual EstadoIdea IdEstadoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<MultimediaIdea> MultimediaIdea { get; set; }
        public virtual ICollection<Puntuacion> Puntuacion { get; set; }
    }
}
