using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Business.Models
{
    public class UsuarioModelBusiness
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Token { get; set; }
    }
}
