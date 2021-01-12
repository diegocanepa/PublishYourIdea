using PublishYourIdea.Api.Application.Contracts.Services;
using PublishYourIdea.Api.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioService usuarioRepository)
        {
            //_adminRepository = adminRepository
            _usuarioRepository = usuarioRepository;
        }

        public Task GetUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }
        /*
        public Task AddUsuario(UsuarioModel usuario)
        {
            _usu
        }*/
    }
}
