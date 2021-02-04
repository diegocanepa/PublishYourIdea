using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishYourIdea.Api.Application.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Config
{
    public static class EmailConfiguration
    {
        public static IServiceCollection AddRegistration(IServiceCollection service, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<Email>();
            
            service.AddSingleton(emailConfig);

            return service;

        }
    }
}
