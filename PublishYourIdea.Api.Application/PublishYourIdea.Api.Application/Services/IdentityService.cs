using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _Iconfiguration;
   

        public string Secret => _Iconfiguration.GetSection("JwtSettings:Secret").Value;
        
        public IdentityService(IUsuarioRepository IUsuarioRepository, IConfiguration Iconfiguration)
        {
            _IUsuarioRepository = IUsuarioRepository;
            _Iconfiguration = Iconfiguration;
        }


        public string Authenticate(string username, string password)
        {
           

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Secret);
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
                Contraseña = password,
                Nombre = "diego",
                Apellido = "canepa",
                FechaCreacion = System.DateTime.Today,
                Token = "asasfas"
            };

            var createdUser = await _IUsuarioRepository.Add(newUser);

            if (createdUser == null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Usuario no creado" }
                };
            }

            return GenerateAuthAuthenticationToken(createdUser);
        }

        public async Task<AuthenticationResultModelBusiness> LoginAsync(string email, string password)
        {
            var user = await _IUsuarioRepository.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Usuario Inexistente" }
                };
            }

            var userHasValidPassword = true;//await _IUsuarioRepository.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Combinacion User/password es incorrecta" }
                };
            }

            return GenerateAuthAuthenticationToken(user);
        }



        private AuthenticationResultModelBusiness GenerateAuthAuthenticationToken(Usuario createdUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, createdUser.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // para refresh token
                    new Claim(JwtRegisteredClaimNames.Email, createdUser.Email),
                    new Claim("Id", createdUser.IdUsuario.ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return new AuthenticationResultModelBusiness
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
