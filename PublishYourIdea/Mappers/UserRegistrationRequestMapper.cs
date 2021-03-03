using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.Models;
using PublishYourIdea.Api.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Mappers
{
    public static class UserRegistrationRequestMapper
    {
        public static UsuarioModelBusiness Map(UserRegistrationRequest dto)
        {
            return new UsuarioModelBusiness()
            {
                Nombre = dto.FirstName,
                Apellido = dto.LastName,
                Email = dto.Email,
                Contraseña = dto.Password,
                FechaCreacion = System.DateTime.UtcNow,
            };
        }

        public static UserRegistrationRequest Map(UsuarioModelBusiness entity)
        {
            return new UserRegistrationRequest()
            {
                FirstName = entity.Nombre,
                LastName = entity.Apellido,
                Email = entity.Email,
                Password = entity.Contraseña,
            };
        }
    }
}
