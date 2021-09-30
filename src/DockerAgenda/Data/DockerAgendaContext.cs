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
        /// Instância do Factory de log da aplicação
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Construtor do contexto da aplicação
        /// </summary>
        /// <param name="options">Configuração do contexto</param>
        /// <param name="loggerFactory">Instância do Factory de log da aplicação</param>
        public DockerAgendaContext(
            DbContextOptions<DockerAgendaContext> options,
            ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

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
                //Habilitando valores dos parâmetros no log
                .EnableSensitiveDataLogging()
                //Habilitando detalhamento exception, somente debug pois tem custo de processamento
                .EnableDetailedErrors()
                //Configurando log
                .UseLoggerFactory(_loggerFactory);
        }

    }
}
