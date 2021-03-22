using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Business.Models
{
    public class IdeaModelBussines
    {
        public int IdIdea { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int IdUsuario { get; set; }
    }
}
