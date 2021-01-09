using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            Comentario = new HashSet<Comentario>();
            Idea = new HashSet<Idea>();
            Puntuacion = new HashSet<Puntuacion>();
            SeguidoresIdUsuarioSeguidoNavigation = new HashSet<Seguidores>();
            SeguidoresIdUsuarioSeguidorNavigation = new HashSet<Seguidores>();
        }

        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public sbyte? Confirmacion { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<Idea> Idea { get; set; }
        public virtual ICollection<Puntuacion> Puntuacion { get; set; }
        public virtual ICollection<Seguidores> SeguidoresIdUsuarioSeguidoNavigation { get; set; }
        public virtual ICollection<Seguidores> SeguidoresIdUsuarioSeguidorNavigation { get; set; }
    }
}
