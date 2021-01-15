using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Application.Configuration
{
    public interface IAppConfig
    {
        int MaxTrys { get; }

        int TimeDelay { get; }
    }
}
