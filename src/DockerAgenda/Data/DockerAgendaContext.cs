using DockerAgenda.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace DockerAgenda.Data
{
    /// <summary>
    /// Contexto da aplicação
    /// </summary>
    public class DockerAgendaContext : DbContext
    {
        /// <summary>
        /// Construtor do contexto da aplicação
        /// </summary>
        /// <param name="options"></param>
        public DockerAgendaContext(DbContextOptions<DockerAgendaContext> options) : base(options)
        { }

        /// <summary>
        /// Tabela de agendas
        /// </summary>
        public DbSet<AgendaEntity> Agendas { get; set; }

        /// <summary>
        /// Tabela de contatos
        /// </summary>
        public DbSet<ContatoEntity> Contatos { get; set; }

        /// <summary>
        /// Tabela de itens do contato
        /// </summary>
        public DbSet<ItemContatoEntity> ItensContatos { get; set; }

        /// <summary>
        /// Configuração do contexto
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Debug);
        }

    }
}
