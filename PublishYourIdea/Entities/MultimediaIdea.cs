using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.Entities
{
    public partial class MultimediaIdea
    {
        public int IdMultimediaIdea { get; set; }
        public int IdIdea { get; set; }
        public string TipoArchivo { get; set; }
        public string Archivo { get; set; }
        public sbyte? Eliminado { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        public virtual Idea IdIdeaNavigation { get; set; }
    }
}
