using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.DataAccess.Mappers
{
    public static class RefreshTokenMapper
    {
        public static RefreshToken Map(RefreshTokenModelBusiness dto)
        {
            return new RefreshToken()
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

        public static RefreshTokenModelBusiness Map(RefreshToken entity)
        {
            return new RefreshTokenModelBusiness()
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
