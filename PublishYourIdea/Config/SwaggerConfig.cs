using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Config
{
    public static class SwaggerConfig
    {
        // Registramos servicios o añadimos configuracion a servicios existentes
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            var basepath = System.AppDomain.CurrentDomain.BaseDirectory; //Obtener donde esta alogado el proyecto
            var xmlPath = Path.Combine(basepath, "PublishYourIdea.Api.xml");
            

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PublishYourIdea API V1", Version = "v1" });
                    //c.IncludeXmlComments(xmlPath); //Acordarse de activar en el proyecto la generacion del archivo (proyecto->propiedades)
                }
            );

            return services;
        }

        //Se añaden los servicios a la aplicacion
        public static IApplicationBuilder AddRegistration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublishYourIdea API V1"));

            return app;

        }
    }
}
