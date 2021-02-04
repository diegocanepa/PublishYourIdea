using PublishYourIdea.Api.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface IIdentityService 
    {
        Task<AuthenticationResultModelBusiness> RegisterAsync(string email, string password);
        Task<AuthenticationResultModelBusiness> LoginAsync(string email, string password);
        Task<AuthenticationResultModelBusiness> RefreshTokenAsync(string token, string refeshToken);
        Task<string> GenerateEmailConfirmationTokenAsync(UsuarioModelBusiness user);
        Task<AuthenticationResultModelBusiness> ConfirmEmail(string userId, string code);
    }
}
