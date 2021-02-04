using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.Mappers
{
    public class EmailConfirmationTokenMapper
    {
        public static EmailConfirmationToken Map(EmailConfirmationTokenModelBussines dto)
        {
            return new EmailConfirmationToken()
            {
                Token = dto.Token,
                JwtId = dto.JwtId,
                CreationDate = dto.CreationDate,
                ExpiryDate = dto.ExpiryDate,
                Used = dto.Used,
                Invalidated = dto.Invalidated,
                UserId = dto.UserId,
            };
        }

        public static EmailConfirmationTokenModelBussines Map(EmailConfirmationToken entity)
        {
            return new EmailConfirmationTokenModelBussines()
            {
                Token = entity.Token,
                JwtId = entity.JwtId,
                CreationDate = (DateTime)entity.CreationDate,
                ExpiryDate = (DateTime)entity.ExpiryDate,
                Used = entity.Used,
                Invalidated = entity.Invalidated,
                UserId = (int)entity.UserId,
            };
        }
    }
}
