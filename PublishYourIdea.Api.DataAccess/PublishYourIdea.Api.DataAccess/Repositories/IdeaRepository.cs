using Microsoft.EntityFrameworkCore;
using PublishYourIdea.Api.DataAccess.Contracts;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IPublishYourIdeaDBContext _publishYourIdeaDBContext;

        public IdeaRepository(IPublishYourIdeaDBContext publishYourIdeaDBContext)
        {
            _publishYourIdeaDBContext = publishYourIdeaDBContext;
        }

        public async Task<Idea> Add(Idea element)
        {
            await _publishYourIdeaDBContext.Idea.AddAsync(element);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return element;
        }

        public async Task<Idea> DeleteAsync(int id)
        {
            Idea idea = await _publishYourIdeaDBContext.Idea.FirstOrDefaultAsync(x => x.IdIdea == id);
            idea.FechaBaja = DateTime.UtcNow;
            idea.ComentarioBaja = "ELIMINADO";

            _publishYourIdeaDBContext.Idea.Update(idea);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return idea;
        }

        public Task<bool> Exist(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Idea>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Idea>> getIdeaByUser(int id)
        {
            List<Idea> Ideas =  (from idea in _publishYourIdeaDBContext.Idea
                                 where idea.IdUsuario == id && idea.FechaBaja == null
                                 orderby idea.Titulo
                                 select idea).ToList();
            return Ideas;
        }

        public async Task<Idea> Update(Idea elemet)
        {
            _publishYourIdeaDBContext.Idea.Update(elemet);
            await _publishYourIdeaDBContext.SaveChangesAsync();

            return elemet;
        }

        public Task<Idea> Update(int id, Idea elemet)
        {
            throw new NotImplementedException();
        }
    }
}
