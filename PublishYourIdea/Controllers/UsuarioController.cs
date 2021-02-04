using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublishYourIdea.Api.Application.Contracts.Services;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")] //[HttpGet("user/data/{id}/rol/{rol}")]
        public async Task<IActionResult> Get(int id)
        {
            var name = await _usuarioService.GetUsuario(id);
            return Ok(name); 
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario([FromBody]UsuarioModel usuario)
        {
            var usuarioAgregado = await _usuarioService.AddUsuario(UsuarioPresentationMapper.Map(usuario));

            return Ok(UsuarioPresentationMapper.Map(usuarioAgregado));
        }
    }
}
