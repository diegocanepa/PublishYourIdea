using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class MultimediaComentario
    {
        public int IdMultimediaComentario { get; set; }
        public int IdComentario { get; set; }
        public string TipoArchivo { get; set; }
        public string Archivo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}
