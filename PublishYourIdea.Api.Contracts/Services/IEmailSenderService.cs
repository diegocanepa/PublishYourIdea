using PublishYourIdea.Api.Application.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface IEmailSenderService
    {
        Task SendEmail(EmailMessage message);

        string GetHTML(string basePathTemplate, object data);
    }
}
