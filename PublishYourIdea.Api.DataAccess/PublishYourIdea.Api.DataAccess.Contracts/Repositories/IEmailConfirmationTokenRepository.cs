using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Contracts.Repositories
{
    public interface IEmailConfirmationTokenRepository : IRepository<EmailConfirmationToken>
    {
        Task<EmailConfirmationToken> Get(string code);

        int GetAllEmailUser(int id);
        Task<EmailConfirmationToken> Update(EmailConfirmationToken emailCode);
    }
}
