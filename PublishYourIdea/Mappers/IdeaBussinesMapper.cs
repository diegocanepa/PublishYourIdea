using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Mappers
{
    public static class IdeaBussinesMapper
    {
        public static IdeaModel Map(IdeaModelBussines dto)
        {
            return new IdeaModel()
            {
                IdIdea = dto.IdIdea,
                IdUsuario = dto.IdUsuario,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaPublicacion = dto.FechaPublicacion,
            };
        }

        public static IdeaModelBussines Map(IdeaModel entity)
        {
            return new IdeaModelBussines()
            {
                IdIdea = entity.IdIdea,
                IdUsuario = entity.IdUsuario,
                Titulo = entity.Titulo,
                Descripcion = entity.Descripcion,
                FechaPublicacion = entity.FechaPublicacion,
                
            };
        }
    }
}
