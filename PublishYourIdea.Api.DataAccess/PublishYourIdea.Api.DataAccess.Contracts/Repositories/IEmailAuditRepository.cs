using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Contracts.Repositories
{
    public interface IEmailAuditRepository
    {
        Task<string> Add(int userId, string statusCode, string action, byte[] contenido);
    }
}
