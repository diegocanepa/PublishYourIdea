using PublishYourIdea.Api.DataAccess.Contracts;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Repositories
{
    public class EmailAuditRepository : IEmailAuditRepository
    {
        private readonly IPublishYourIdeaDBContext _publishYourIdeaDBContext;

        public EmailAuditRepository(IPublishYourIdeaDBContext publishYourIdeaDBContext)
        {
            _publishYourIdeaDBContext = publishYourIdeaDBContext;
        }

        public async Task<string> Add(int userId, string statusCode, string action, byte[] contenido )
        {
            var EmailNotification = new EmailNotificacion
            {
                IdEmailNotificacion = Guid.NewGuid().ToString(),
                UserId = userId,
                Status = statusCode,
                Action = action,
                Contenido = contenido
            };

            await _publishYourIdeaDBContext.EmailNotificacion.AddAsync(EmailNotification);
            await _publishYourIdeaDBContext.SaveChangesAsync();

            return EmailNotification.IdEmailNotificacion;
        }
    }
}
