using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Contracts.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> Update(Usuario entity);

        Task<Usuario> FindByEmailAsync(string email);
        bool CheckPasswordAsync(string passwordHased, string password);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);

        Task<RefreshToken> UpdateRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken);
    }
}
