using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using PublishYourIdea.Api.DataAccess.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _ideaRepository;

        public IdeaService(IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }


        public async Task<List<IdeaModelBussines>>  getIdeasByUser(int id)
        {
            List<Idea> ideas = await _ideaRepository.getIdeaByUser(id);
            List<IdeaModelBussines> ideasModelBussines = new List<IdeaModelBussines>();
            foreach(var idea in ideas)
            {
                ideasModelBussines.Add(IdeaEntityMapper.Map(idea));
            }

            return ideasModelBussines;
        }

        public async Task<IdeaModelBussines> deleteIdea(int idIdea)
        {
            return IdeaEntityMapper.Map( await _ideaRepository.DeleteAsync(idIdea));
        }

        public async Task<IdeaModelBussines> updateIdea(IdeaModelBussines idea)
        {
            return IdeaEntityMapper.Map(await _ideaRepository.Update(IdeaEntityMapper.Map(idea)));
        }

        public async Task<IdeaModelBussines> addIdea(IdeaModelBussines idea)
        {
            return IdeaEntityMapper.Map(await _ideaRepository.Add(IdeaEntityMapper.Map(idea)));
        }
    }
}
