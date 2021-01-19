using Microsoft.IdentityModel.Tokens;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly IUsuarioRepository _IUsuarioRepository;
        //private readonly JwtSettings

        public IdentityService(IUsuarioRepository IUsuarioRepository)
        {
            _IUsuarioRepository = IUsuarioRepository;
        }


        public string Authenticate(string username, string password)
        {
           

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return  tokenHandler.WriteToken(token);
        }

        public async Task<AuthenticationResultModelBusiness> RegisterAsync(string email, string password)
        {
            var existingUser = await _IUsuarioRepository.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Usuario con email ya existente" }
                };
            }

            var newUser = new Usuario
            {
                Email = email,
                Contraseña = password
            };

            var createdUser = await _IUsuarioRepository.Add(newUser);

            if (createdUser == null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Usuario no creado" }
                };
            }


            return new AuthenticationResultModelBusiness
            {

            };
        }
    }
}
