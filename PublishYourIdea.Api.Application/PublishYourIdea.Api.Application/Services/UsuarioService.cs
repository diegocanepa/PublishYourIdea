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
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string> GetUsuario(int idUsuario)
        {
            var entidad = await _usuarioRepository.Get(idUsuario);

            return entidad.Nombre;
        }
        
        public async Task<UsuarioModelBusiness> AddUsuario(UsuarioModelBusiness usuario)
        {
            var addedEntity = await _usuarioRepository.Add(UsuarioMapper.Map(usuario));
            return UsuarioMapper.Map(addedEntity);
        }

        /*
        public async Task<UsuarioModel> AddUsuario(UsuarioModel usuario)
        {
           var addedEntity = await _usuarioRepository.Add(UsuarioMapper.Map(usuario));
           return UsuarioMapper.Map(addedEntity);
        }*/
    }
}
