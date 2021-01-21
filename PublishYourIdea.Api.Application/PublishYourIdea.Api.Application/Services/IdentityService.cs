using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using PublishYourIdea.Api.DataAccess.Mappers;
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
        public TimeSpan TokenLifetime => TimeSpan.Parse(_Iconfiguration.GetSection("JwtSettings:TokenLifetime").Value);

        private TokenValidationParameters _tokenValidationParameters;

        public IdentityService(IUsuarioRepository IUsuarioRepository,
                                IConfiguration Iconfiguration)
        {
            _IUsuarioRepository = IUsuarioRepository;
            _Iconfiguration = Iconfiguration;
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = false
            };
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
            return tokenHandler.WriteToken(token);
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

            return await GenerateAuthAuthenticationTokenAsync(createdUser);
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

            var userHasValidPassword = _IUsuarioRepository.CheckPasswordAsync(user.Contraseña, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Combinacion User/password es incorrecta" }
                };
            }

            return await GenerateAuthAuthenticationTokenAsync(user);
        }

        public async Task<AuthenticationResultModelBusiness> RefreshTokenAsync(string token, string refeshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "Invalid Token" } };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "This token hasn´t expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storeRefreshToken = RefreshTokenMapper.Map(await _IUsuarioRepository.GetRefreshTokenAsync(refeshToken));

            if (storeRefreshToken == null)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "This refresh Token does not exist" } };
            }

            if (DateTime.UtcNow > storeRefreshToken.ExpiryDate)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "This refresh Token has expired" } };
            }

            if (storeRefreshToken.Invalidated != null)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "This refresh Token has been invalidated" } };
            }

            if (storeRefreshToken.Used != null)
            {
                return new AuthenticationResultModelBusiness { Errors = new[] { "This refresh Token has been used" } };
            }

            if (storeRefreshToken.JwtId != jti) { return new AuthenticationResultModelBusiness { Errors = new[] { "This refreshToken does not match this JWT" } }; }


            storeRefreshToken.Used = "S";
            storeRefreshToken = RefreshTokenMapper.Map(await _IUsuarioRepository.UpdateRefreshToken(RefreshTokenMapper.Map(storeRefreshToken)));
            var user = await _IUsuarioRepository.FindByEmailAsync(validatedToken.Claims.SingleOrDefault(x => x.Type == "Email").Value);
            return await GenerateAuthAuthenticationTokenAsync(user);
        }


        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm (SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResultModelBusiness> GenerateAuthAuthenticationTokenAsync(Usuario createdUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, createdUser.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // para refresh token
                    new Claim("Email", createdUser.Email),
                    new Claim("Id", createdUser.IdUsuario.ToString())

                }),
                Expires = DateTime.UtcNow.AddSeconds(45),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshTokenModelBusiness
            {
                JwtId = token.Id,
                UserId = createdUser.IdUsuario,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString()
            };


            refreshToken = RefreshTokenMapper.Map(await _IUsuarioRepository.AddRefreshTokenAsync(RefreshTokenMapper.Map(refreshToken)));

            return new AuthenticationResultModelBusiness
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
