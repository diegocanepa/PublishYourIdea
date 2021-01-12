using Microsoft.Extensions.DependencyInjection;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Application.Services;
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

            return services;
        }

        private static IServiceCollection AddRegisterServices(IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();


            return services;
        }

        private static IServiceCollection AddRegisterRepository(IServiceCollection services)
        {
            //Debemos poner uno por cada repositorio que tengamos
            services.AddTransient<IUsuarioRepository, UsuarioRepository>(); //IUsuarioRepository IMPLEMENTA UsuarioRepository 
            
            
            return services;
        }
    }
}
