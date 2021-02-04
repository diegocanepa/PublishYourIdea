using PublishYourIdea.Api.Business.Models;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(int idUsuario);
        Task<UsuarioModelBusiness> AddUsuario(UsuarioModelBusiness usuario);

        Task<UsuarioModelBusiness> GetUsuarioByEmail(string email);
    }
}
