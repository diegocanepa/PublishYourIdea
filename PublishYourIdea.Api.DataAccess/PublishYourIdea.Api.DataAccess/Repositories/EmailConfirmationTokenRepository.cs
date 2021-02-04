using PublishYourIdea.Api.DataAccess.Contracts;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PublishYourIdea.Api.DataAccess.Repositories
{
    public class EmailConfirmationTokenRepository : IEmailConfirmationTokenRepository
    {
        private readonly IPublishYourIdeaDBContext _publishYourIdeaDBContext;

        public EmailConfirmationTokenRepository(IPublishYourIdeaDBContext publishYourIdeaDBContext)
        {
            _publishYourIdeaDBContext = publishYourIdeaDBContext;
        }

        public async Task<EmailConfirmationToken> Add(EmailConfirmationToken emailConfirmationToken)
        {
             _publishYourIdeaDBContext.EmailConfirmationToken.Add(emailConfirmationToken);
            await _publishYourIdeaDBContext.SaveChangesAsync();

            return emailConfirmationToken;
        }

        public async Task<EmailConfirmationToken> Get(string code)
        {
            return await _publishYourIdeaDBContext.EmailConfirmationToken.FirstOrDefaultAsync(x => x.Token == code);
        }


        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(int id)
        {
            throw new NotImplementedException();
        }

        public Task<EmailConfirmationToken> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmailConfirmationToken>> GetAll()
        {
            throw new NotImplementedException();
        }


        public int GetAllEmailUser(int id)
        {
            return _publishYourIdeaDBContext.EmailConfirmationToken.Where(x => x.UserId == id).Count();
        }

        public Task<EmailConfirmationToken> Update(int id, EmailConfirmationToken elemet)
        {
            throw new NotImplementedException();
        }

        public async Task<EmailConfirmationToken> Update(EmailConfirmationToken entity)
        {
            var updateEntity = _publishYourIdeaDBContext.EmailConfirmationToken.Update(entity);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return updateEntity.Entity;
        }
    }
}
