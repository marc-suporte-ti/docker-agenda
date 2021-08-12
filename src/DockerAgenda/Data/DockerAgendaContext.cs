using DockerAgenda.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace DockerAgenda.Data
{
    public class DockerAgendaContext : DbContext
    {
        public DbSet<AgendaEntity> Agendas { get; set; }

        public DbSet<ContatoEntity> Contatos { get; set; }

        public DbSet<ItemContatoEntity> ItensContatos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Debug);
        }

    }
}
