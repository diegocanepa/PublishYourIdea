using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Contracts.Repositories
{
    public interface IIdeaRepository : IRepository<Idea>
    {
        Task<List<Idea>> getIdeaByUser(int id);
        Task<Idea> Update(Idea elemet);
    }
}
