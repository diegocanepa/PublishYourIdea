using System;
using System.Collections.Generic;

namespace PublishYourIdea.Api.DataAccess.Contracts.Entities
{
    public partial class Seguidores
    {
        public int IdSeguidores { get; set; }
        public int IdUsuarioSeguidor { get; set; }
        public int IdUsuarioSeguido { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
        public sbyte? FueAceptado { get; set; }

        public virtual Usuario IdUsuarioSeguidoNavigation { get; set; }
        public virtual Usuario IdUsuarioSeguidorNavigation { get; set; }
    }
}
