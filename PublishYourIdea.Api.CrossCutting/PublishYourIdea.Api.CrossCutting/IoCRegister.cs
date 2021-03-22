using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PublishYourIdea.Api.Application.Configuration;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Application.Services;
using PublishYourIdea.Api.DataAccess;
using PublishYourIdea.Api.DataAccess.Contracts;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using PublishYourIdea.Api.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.CrossCutting
{
    public static class IoCRegister
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            AddRegisterServices(services);
            AddRegisterRepository(services);
            AddRegisterOthers(services);

            return services;
        }

        private static IServiceCollection AddRegisterServices(IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<ILoggerManagerService, LoggerManagerService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IPasswordHasherService,PasswordHasherService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IIdeaService, IdeaService>();

            return services;
        }

        private static IServiceCollection AddRegisterRepository(IServiceCollection services)
        {
            //Debemos poner uno por cada repositorio que tengamos
            services.AddTransient<IUsuarioRepository, UsuarioRepository>(); //IUsuarioRepository IMPLEMENTA UsuarioRepository 
            services.AddTransient<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
            services.AddTransient<IEmailAuditRepository, EmailAuditRepository>();
            services.AddTransient<IIdeaRepository, IdeaRepository>();

            return services;
        }

        private static IServiceCollection AddRegisterOthers(IServiceCollection services)
        {
            services.AddTransient<IAppConfig, AppConfig>();
            services.AddTransient<IPublishYourIdeaDBContext, PublishYourIdeaDBContext>();

            return services;
        }
    }
}
