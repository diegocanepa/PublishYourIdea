using PublishYourIdea.Api.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface IUsuarioService
    {
        Task<string> GetUsuario(int idUsuario);
        Task<UsuarioModelBusiness> AddUsuario(UsuarioModelBusiness usuario);
    }
}
