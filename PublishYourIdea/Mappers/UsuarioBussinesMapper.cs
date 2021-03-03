using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Mappers
{
    public static class UsuarioBussinesMapper
    {
        public static UsuarioModel Map(UsuarioModelBusiness dto)
        {
            return new UsuarioModel()
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
            };
        }

        public static UsuarioModelBusiness Map(UsuarioModel entity)
        {
            return new UsuarioModelBusiness()
            {
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Email = entity.Email,
            };
        }
    }
}
