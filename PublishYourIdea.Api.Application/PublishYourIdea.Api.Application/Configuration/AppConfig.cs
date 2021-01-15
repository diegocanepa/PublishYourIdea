using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Application.Configuration
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration _configuration;

        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int MaxTrys => int.Parse(_configuration.GetSection("Polly:MaxTrys").Value);
        public int TimeDelay => int.Parse(_configuration.GetSection("Polly:TimeDelay").Value);
    }
}
