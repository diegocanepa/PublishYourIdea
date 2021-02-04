
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
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _IIdentityService.RegisterAsync(request.Email, request.Password);


            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }


            var user = await _usuarioService.GetUsuarioByEmail(request.Email);
            await SendEmailConfirmationTokenAsync(user);

            return Ok(new SuccessDetailsModels
            {
                Status = 200,
                Message = authResponse.message
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {

            var authResponse = await _IIdentityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefeshToken = authResponse.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> refreshToken([FromBody] RefreshTokenRequest request)
        {

            var authResponse = await _IIdentityService.RefreshTokenAsync(request.Token, request.RefeshToken);


            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefeshToken = authResponse.RefreshToken
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

            var message = new EmailMessage(new string[] { "diego.canepa241198@gmail.com" }, "Validacion Email", content);

            await _emailSenderService.SendEmail(message);

            return true;
        }


        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var authResponse = await _IIdentityService.ConfirmEmail(userId, code);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new SuccessDetailsModels
            {
                Status = 200,
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
                return BadRequest(new AuthFailedResponse
                {
                    Errors = new[] { "Ya se le enviarion varios mails con anterioridad" }
                });
            }

            return Ok(new SuccessDetailsModels
            {
                Status = 200,
                Message = "Mail reenviado"
            });
        }


    }
}
