using HandlebarsDotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using NPOI.Util;
using PublishYourIdea.Api.Application.Contracts.Models;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly Email _emailConfig;
        private readonly IEmailAuditRepository _emailAuditRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public EmailSenderService(Email email, IEmailAuditRepository emailAuditRepository, IUsuarioRepository usuarioRepository)
        {
            _emailConfig = email;
            _emailAuditRepository = emailAuditRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            var user = await _usuarioRepository.FindByEmailAsync(emailMessage.To.ToString());

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XQAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(emailMessage);

                    //cargamos auditoria
                    ByteArrayOutputStream baos = new ByteArrayOutputStream();
                    (emailMessage.Body).WriteTo(baos);
                    byte[] bytes = baos.ToByteArray();
                    await _emailAuditRepository.Add(user.IdUsuario, "200", emailMessage.Subject, bytes);
                }
                catch (Exception ex)
                {
                    System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
                    await _emailAuditRepository.Add(user.IdUsuario, "400", emailMessage.Subject, codificador.GetBytes(ex.Message));

                    throw new Exception(ex.Message); 
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();             
                }
            }
        }

        public string GetHTML(string basePathTemplate, object data)
        {
            if (!File.Exists(basePathTemplate))
            {
                throw new Exception(string.Format("La plantilla {0} no se ha encontrado en el folder de plantillas", basePathTemplate));
            }

            var templateText = File.ReadAllText(basePathTemplate);
            var template = Handlebars.Compile(templateText);

            return template(data);
        }
    }
}
