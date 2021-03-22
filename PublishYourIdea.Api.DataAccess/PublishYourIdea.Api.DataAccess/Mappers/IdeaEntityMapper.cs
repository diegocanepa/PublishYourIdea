using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.Mappers
{
    public static class IdeaEntityMapper
    {
        public static Idea Map(IdeaModelBussines dto)
        {
            return new Idea()
            {
                IdIdea = dto.IdIdea,
                IdUsuario = dto.IdUsuario,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaPublicacion = dto.FechaPublicacion,
            };
        }

        public static IdeaModelBussines Map(Idea entity)
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
