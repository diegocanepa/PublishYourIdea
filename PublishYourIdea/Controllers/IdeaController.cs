using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using PublishYourIdea.Api.Mappers;
using PublishYourIdea.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly IIdeaService _ideaService;

        public IdeaController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> getIdeasByUser(int idUser)
        {
            List<IdeaModelBussines> ideas = await _ideaService.getIdeasByUser(idUser);

            List<IdeaModel> ideasModel = new List<IdeaModel>();
            foreach (var idea in ideas)
            {
                ideasModel.Add(IdeaBussinesMapper.Map(idea));
            }

            return Ok(ideasModel);
        }

        [HttpDelete("{idIdea}")]
        public async Task<IActionResult> delete(int idIdea)
        {
            IdeaModel idea = IdeaBussinesMapper.Map(await _ideaService.deleteIdea(idIdea));

            return Ok(idea);
        }

        [HttpPost]
        public async Task<IActionResult> add([FromBody] IdeaModel idea)
        {
            IdeaModel addIdea = IdeaBussinesMapper.Map(await _ideaService.addIdea(IdeaBussinesMapper.Map(idea)));

            return Ok(addIdea);
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] IdeaModel idea)
        {
            IdeaModel updatedIdea = IdeaBussinesMapper.Map(await _ideaService.updateIdea(IdeaBussinesMapper.Map(idea)));

            return Ok(updatedIdea);
        }

    }
}
