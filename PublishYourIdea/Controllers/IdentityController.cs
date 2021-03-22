
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublishYourIdea.Api.Application.Contracts.Models;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.Models;
using PublishYourIdea.Api.Models.Request;
using PublishYourIdea.Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PublishYourIdea.Api.DataAccess.Mappers;
using PublishYourIdea.Api.Mappers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublishYourIdea.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _IIdentityService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IUsuarioService _usuarioService;

        public IdentityController(IIdentityService identityService, IEmailSenderService emailSenderService, IUsuarioService usuarioService)
        {
            _IIdentityService = identityService;
            _emailSenderService = emailSenderService;
            _usuarioService = usuarioService;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {

            var authResponse = await _IIdentityService.RegisterAsync(UserRegistrationRequestMapper.Map(request));

            if (authResponse.Success)
            {
                var user = await _usuarioService.GetUsuarioByEmail(request.Email);
                await SendEmailConfirmationTokenAsync(user);
            }

            return Ok(new RegisterResponseModels
            {
                Errors = authResponse.Errors,
                Success = authResponse.Success,
                Message = authResponse.message,
                user = authResponse.user is null ? null : UsuarioBussinesMapper.Map(authResponse.user)
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _IIdentityService.LoginAsync(request.Email, request.Password);

            return Ok(new AuthResponseModel
            {
                success = authResponse.Success,
                Errors = authResponse.Errors,
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken,
                user = authResponse.user is null ? null : UsuarioBussinesMapper.Map(authResponse.user),
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> refreshToken([FromBody] RefreshTokenRequest request)
        {

            var authResponse = await _IIdentityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            return Ok(new AuthResponseModel
            {
                success = authResponse.Success,
                Errors = authResponse.Errors,
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("SendConfrimationEmail")]
        public async Task<bool> SendEmailConfirmationTokenAsync(UsuarioModelBusiness user)
        {
           
            var code = await _IIdentityService.GenerateEmailConfirmationTokenAsync(user);

            if (code is null)
            {
                return false;
            }

            var callBackUrl = Url.Action(nameof(ConfirmEmail), "Identity", new { UserId = user.IdUsuario, code = code }, Request.Scheme);

            var data = new { UserName = string.Concat(user.Nombre, " ", user.Apellido), CallBackUrl = callBackUrl };
            var basePathTemplate = String.Concat(Directory.GetParent(Directory.GetCurrentDirectory().ToString()), "/PublishYourIdea.Api.Application/PublishYourIdea.Api.Application/EmailTemplates/RegisterEmailModel.html");
            var content = _emailSenderService.GetHTML(basePathTemplate, data);

            var message = new EmailMessage(new string[] { user.Email }, "Validacion Email", content);

            await _emailSenderService.SendEmail(message);

            return true;
        }


        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var authResponse = await _IIdentityService.ConfirmEmail(userId, code);

            return Ok(new RegisterResponseModels
            {
                Success = authResponse.Success,
                Errors = authResponse.Errors,
                Message = authResponse.message
            });
        }


        [AllowAnonymous]
        [HttpPost("ResendEmail")]
        public async Task<IActionResult> ResendEmail([FromBody] UserLoginRequest request)
        {
            var user = await _usuarioService.GetUsuarioByEmail(request.Email);
            var emailSend = await SendEmailConfirmationTokenAsync(user);

            if (!emailSend)
            {
                return Ok(new EmailResponseModel
                {
                    Success = false,
                    Errors = new[] { "Ya se le enviarion varios mails con anterioridad" }
                });
            }

            return Ok(new EmailResponseModel
            {
                StatusCode = 200,
                Success = true,
                Message = "Mail reenviado"
            });
        }


    }
}
