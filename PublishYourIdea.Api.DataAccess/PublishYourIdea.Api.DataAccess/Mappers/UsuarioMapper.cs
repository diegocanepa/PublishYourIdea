using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario Map(UsuarioModel dto)
        {
            return new Usuario()
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Contraseña = dto.Contraseña,
                FechaCreacion = dto.FechaCreacion,
                Token = dto.Token
            };
        }

        public static UsuarioModel Map(Usuario entity)
        {
            return new UsuarioModel()
            {
                IdUsuario = entity.IdUsuario,
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
