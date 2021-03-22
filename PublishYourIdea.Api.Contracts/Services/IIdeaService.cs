using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface IIdeaService
    {
        Task<List<IdeaModelBussines>> getIdeasByUser(int id);
        Task<IdeaModelBussines> updateIdea(IdeaModelBussines idea);
        Task<IdeaModelBussines> deleteIdea(int idIdea);
        Task<IdeaModelBussines> addIdea(IdeaModelBussines idea);
    }
}
