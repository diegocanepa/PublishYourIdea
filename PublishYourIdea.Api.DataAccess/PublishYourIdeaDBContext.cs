using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PublishYourIdea.Api.DataAccess;


namespace PublishYourIdea.Api.DataAccess
{
    public class PublishYourIdeaDBContext : DbContext
    {

        public PublishYourIdeaDBContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
