﻿using Microsoft.EntityFrameworkCore;
using PublishYourIdea.Api.DataAccess.Contracts;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;
using PublishYourIdea.Api.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishYourIdea.Api.DataAccess.Repositories
{
    public class UsuarioRepository: IUsuarioRepository
    {
        //CRUD --> CREATE READ UPDATE DELETE

        private readonly IPublishYourIdeaDBContext _publishYourIdeaDBContext;

        public UsuarioRepository(IPublishYourIdeaDBContext publishYourIdeaDBContext)
        {
            _publishYourIdeaDBContext = publishYourIdeaDBContext;
        }

        //Task es para los hilos y sincronia en C#
        public async Task<Usuario> Add(Usuario entity)
        {
            await _publishYourIdeaDBContext.Usuario.AddAsync(entity);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return entity; 
        }

        public async Task<Usuario> Get(int idEntity)
        {
            return await _publishYourIdeaDBContext.Usuario.FirstOrDefaultAsync(x=> x.IdUsuario == idEntity);
        }

        public async Task<Usuario> Update(Usuario entity)
        {
            var updateEntity = _publishYourIdeaDBContext.Usuario.Update(entity);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return updateEntity.Entity;
        }

        public async Task<Usuario> Update(int idEntity, Usuario updateEntity)
        {
            var entity = await Get(idEntity);
            entity.Nombre = updateEntity.Nombre;

            _publishYourIdeaDBContext.Usuario.Update(entity);
            await _publishYourIdeaDBContext.SaveChangesAsync();
            return entity;
        }
        
        public Task<bool> Exist(int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return _publishYourIdeaDBContext.Usuario.Select(x => x);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _publishYourIdeaDBContext.Usuario.SingleAsync(x => x.IdUsuario == id);
            _publishYourIdeaDBContext.Usuario.Remove(entity);
            await _publishYourIdeaDBContext.SaveChangesAsync();
        }
    }
}
