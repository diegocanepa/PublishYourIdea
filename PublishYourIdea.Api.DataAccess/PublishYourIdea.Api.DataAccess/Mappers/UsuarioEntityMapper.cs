using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.Mappers
{
    public static class UsuarioEntityMapper
    {
        public static Usuario Map(UsuarioModelBusiness dto)
        {
            return new Usuario()
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Contraseña = dto.Contraseña,
                FechaCreacion = dto.FechaCreacion,
            };
        }

        public static UsuarioModelBusiness Map(Usuario entity)
        {
            return new UsuarioModelBusiness()
            {
                IdUsuario = entity.IdUsuario,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Email = entity.Email,
                Contraseña = entity.Contraseña,
                FechaCreacion = entity.FechaCreacion,
            };
        }


    }
}
