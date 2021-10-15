using DockerAgenda.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DockerAgendaContext _db;

        /// <summary>
        /// Construtor do controller responsável por processar migrações pendentes
        /// </summary>
        /// <param name="db">Instância de banco de dados da aplicação</param>
        public MigrationsController(DockerAgendaContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Validando se existe pacote não aplicado no banco
        /// </summary>
        /// <returns>Resposta processamento</returns>
        [HttpGet]
        public async Task<ActionResult> ValidarMigration()
        {
            var migracoesPendentes = await _db.Database.GetPendingMigrationsAsync();

            if (migracoesPendentes?.Any() == true)
            {
                foreach (var migracao in migracoesPendentes)
                {
                    Console.WriteLine($"Migração: {migracao}");
                }

                await _db.Database.MigrateAsync();
            }

            // Adiantando abertura da conexão
            _db.Database.GetDbConnection().Open();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            return Ok();
        }
    }
}
