using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class EstadoIdea
    {
        public EstadoIdea()
        {
            Idea = new HashSet<Idea>();
        }

        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string DescripcionEstado { get; set; }

        public virtual ICollection<Idea> Idea { get; set; }
    }
}
