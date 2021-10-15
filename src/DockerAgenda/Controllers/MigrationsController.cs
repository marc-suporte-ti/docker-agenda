using DockerAgenda.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAgenda.Controllers
{
    /// <summary>
    /// Controller responsável por processar migrações pendentes
    /// </summary>
    [Produces("application/json", "application/xml")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/migrations")]
    public class MigrationsController : Base
    {
        private readonly IApplicationBuilder _app;

        /// <summary>
        /// Construtor do controller responsável por processar migrações pendentes
        /// </summary>
        /// <param name="app">Instância para configuração do pipeline de uma solicitação</param>
        public MigrationsController(IApplicationBuilder app)
        {
            _app = app;
        }

        /// <summary>
        /// Validando se existe pacote não aplicado no banco
        /// </summary>
        /// <returns>Resposta processamento</returns>
        [HttpPost("{id}/contatos/{idContato}")]
        public async Task<ActionResult> PostItemContato()
        {
            using var db = _app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DockerAgendaContext>();

            var migracoesPendentes = await db.Database.GetPendingMigrationsAsync();

            if (migracoesPendentes?.Any() == true)
            {
                foreach (var migracao in migracoesPendentes)
                {
                    Console.WriteLine($"Migração: {migracao}");
                }

                await db.Database.MigrateAsync();
            }

            // Adiantando abertura da conexão
            db.Database.GetDbConnection().Open();
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            return Ok();
        }
    }
}
