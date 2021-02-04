using Polly;
using PublishYourIdea.Api.Application.Configuration;
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
        private readonly IAppConfig _appConfig;
        public UsuarioService(IUsuarioRepository usuarioRepository, IAppConfig appConfig)
        {
            _usuarioRepository = usuarioRepository;
            _appConfig = appConfig;
        }

        public async Task<Usuario> GetUsuario(int idUsuario)
        {
            var entidad = await _usuarioRepository.Get(idUsuario);

            return entidad;
        }

        public async Task<UsuarioModelBusiness> GetUsuarioByEmail(string email)
        {
            var entity = await _usuarioRepository.FindByEmailAsync(email);

            return UsuarioMapper.Map(entity);
        }

        public async Task<UsuarioModelBusiness> AddUsuario(UsuarioModelBusiness usuario)
        {
            var maxTrys = _appConfig.MaxTrys;
            var timeToWait = TimeSpan.FromSeconds(_appConfig.TimeDelay);

            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i=>timeToWait); //VER TODAS LAS CONFIGURACIONES DISPONIBLES

            return await retryPolicy.ExecuteAsync( async () => {
                var addedEntity = await _usuarioRepository.Add(UsuarioMapper.Map(usuario));
                return UsuarioMapper.Map(addedEntity);
            }); 
        }
    }
}
