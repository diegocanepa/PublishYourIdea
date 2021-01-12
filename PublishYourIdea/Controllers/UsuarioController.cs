using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublishYourIdea.Api.Application.Contracts.Services;
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
        
        //[HttpGet("user/data/{id}/rol/{rol}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var name = await _usuarioService.GetUsuario(id);
            return Ok(name); //devuelve un 200
        }
    }
}
