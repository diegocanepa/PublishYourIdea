using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.EntityConfig
{
    class UsuarioEntityConfig
    {

        public static void SetEntityBuilder(EntityTypeBuilder<Usuario> entityBuilder)
        {
            //Por si se llama diferente la tabla en la BD que la entidad creada.
            entityBuilder.ToTable("Usuario");

            //Para determinar la clave primaria
            entityBuilder.HasKey(x => x.IdUsuario);

            //Para determinar campos obligatorios
            entityBuilder.Property(x => x.IdUsuario).IsRequired();
            entityBuilder.Property(x => x.Nombre).IsRequired();
            entityBuilder.Property(x => x.Apellido).IsRequired();
            entityBuilder.Property(x => x.Email).IsRequired();
            entityBuilder.Property(x => x.Contraseña).IsRequired();
            entityBuilder.Property(x => x.FechaCreacion).IsRequired();

            //Si tuviese una relacion con otra tabla
            //entityBuilder.HasOne(x => x.Comentario).WhithOne(x => x.idComentario);

        }
    }
}
