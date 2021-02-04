using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Contracts
{
    public interface IPublishYourIdeaDBContext
    {
        DbSet<Comentario> Comentario { get; set; }
        DbSet<EstadoIdea> EstadoIdea { get; set; }
        DbSet<Idea> Idea { get; set; }
        DbSet<MultimediaComentario> MultimediaComentario { get; set; }
        DbSet<MultimediaIdea> MultimediaIdea { get; set; }
        DbSet<Puntuacion> Puntuacion { get; set; }
        DbSet<Seguidores> Seguidores { get; set; }
        DbSet<Usuario> Usuario { get; set; }
        DbSet<RefreshToken> RefreshToken { get; set; }
        DbSet<EmailConfirmationToken> EmailConfirmationToken { get; set; }
        DbSet<EmailNotificacion> EmailNotificacion { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        void RemoveRange(IEnumerable<object> entities);
        EntityEntry Update(object entity);
    }
}
