using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Mappers
{
    public static class UsuarioPresentationMapper
    {
        public static UsuarioModelBusiness Map(UsuarioModel dto)
        {
            return new UsuarioModelBusiness()
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Contraseña = dto.Contraseña,
                FechaCreacion = dto.FechaCreacion,
                Token = dto.Token
            };
        }

        public static UsuarioModel Map(UsuarioModelBusiness entity)
        {
            return new UsuarioModel()
            {
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Email = entity.Email,
                Contraseña = entity.Contraseña,
                FechaCreacion = entity.FechaCreacion,
                Token = entity.Token
            };
        }
    }
}
