using Microsoft.AspNetCore.Identity;
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
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailConfirmationTokenRepository _emailConfirmationToken;

        public string Secret => _Iconfiguration.GetSection("JwtSettings:Secret").Value;
        public TimeSpan TokenLifetime => TimeSpan.Parse(_Iconfiguration.GetSection("JwtSettings:TokenLifetime").Value);

        private TokenValidationParameters _tokenValidationParameters;

        public IdentityService(IUsuarioRepository IUsuarioRepository,
                                IConfiguration Iconfiguration,
                                IPasswordHasherService passwordHasherService,
                                IEmailConfirmationTokenRepository emailConfirmationToken)
        {
            _IUsuarioRepository = IUsuarioRepository;
            _Iconfiguration = Iconfiguration;
            _passwordHasherService = passwordHasherService;
            _emailConfirmationToken = emailConfirmationToken;
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

        public async Task<AuthenticationResultModelBusiness> RegisterAsync(UsuarioModelBusiness user)
        {
            var existingUser = await _IUsuarioRepository.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Success = false,
                    Errors = new[] { "This user's email address already exists" }
                };
            }

            var newUser = new Usuario
            {
                Email = user.Email,
                Contraseña = _passwordHasherService.HashPassword(user.Contraseña),
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                FechaCreacion = user.FechaCreacion,
            };

            var createdUser = await _IUsuarioRepository.Add(newUser);

            if (createdUser == null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Success = false,
                    Errors = new[] { "Something went wrong - Couldn't create the user. Please try again later." }
                };
            }

            return new AuthenticationResultModelBusiness
            {
                Success = true,
                message = "User created. Check your email and confirm your account, you must be confirmed before you can log in",
                user = UsuarioEntityMapper.Map(createdUser)
            };
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

            if (user.Confirmacion is null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "User account not confirimed. Please check your email, you must have a confirmd email to log on" }
                };
            }

            PasswordVerificationResult userHasValidPassword = _passwordHasherService.VerifyHashedPassword(user.Contraseña, password);

            if (userHasValidPassword == PasswordVerificationResult.Failed)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Combination User/password is incorrect" }
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(UsuarioModelBusiness user)
        {
            var cantEmailSended = _emailConfirmationToken.GetAllEmailUser(user.IdUsuario); 

            if (cantEmailSended > 2)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // para refresh token
                    new Claim("Email", user.Email),
                    new Claim("Id", user.IdUsuario.ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var emailConfirmToken = new EmailConfirmationTokenModelBussines
            {
                JwtId = token.Id,
                UserId = user.IdUsuario,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(15),
                Token = Guid.NewGuid().ToString()
            };

            await _emailConfirmationToken.Add(EmailConfirmationTokenMapper.Map(emailConfirmToken));

            return emailConfirmToken.Token;
        }

        public async Task<AuthenticationResultModelBusiness> ConfirmEmail(string userId, string code) 
        {
            var usuario = await _IUsuarioRepository.Get(int.Parse(userId));
            var emailCode = await _emailConfirmationToken.Get(code);

            if (usuario is null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "User not exist" }
                };
            }

            if (emailCode is null)
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Email code not exist" }
                };
            }

            if(usuario.IdUsuario == emailCode.UserId)
            {
                if (emailCode.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthenticationResultModelBusiness { Errors = new[] { "This Email Token is expired. Please try to send the email again" } };
                }

                if (emailCode.Invalidated != null)
                {
                    return new AuthenticationResultModelBusiness { Errors = new[] { "This Email Toekn is invalidate. Please try to send the email again" } };
                }

                if (emailCode.Used != null)
                {
                    return new AuthenticationResultModelBusiness { Errors = new[] { "This email Token has been used. Please try to send the email again" } };
                }

                usuario.Confirmacion = "S";
                emailCode.Used = "S";
                await _IUsuarioRepository.Update(usuario);
                await _emailConfirmationToken.Update(emailCode);
            }
            else
            {
                return new AuthenticationResultModelBusiness
                {
                    Errors = new[] { "Code and User not match" }
                };
            }

            return new AuthenticationResultModelBusiness { Success = true, message = "Email confirmed" };

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
                Expires = DateTime.UtcNow.AddMinutes(90),
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
                ExpiryDate = DateTime.UtcNow.AddMinutes(180),
                Token = Guid.NewGuid().ToString()
            };


            refreshToken = RefreshTokenMapper.Map(await _IUsuarioRepository.AddRefreshTokenAsync(RefreshTokenMapper.Map(refreshToken)));

            return new AuthenticationResultModelBusiness
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                user = UsuarioEntityMapper.Map(createdUser)
            };
        }


    }
}
