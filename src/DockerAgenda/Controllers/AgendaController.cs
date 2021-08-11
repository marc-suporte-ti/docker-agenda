using DockerAgenda.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DockerAgenda.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/agenda")]
    public class AgendaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AgendaController> _logger;

        public AgendaController(ILogger<AgendaController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Responsável por criar a agenda para o novo usuário
        /// </summary>
        /// <param name="agendaRequestDto">Dados para registro da agenda</param>
        /// <returns>Agenda criada</returns>
        [HttpPost]
        public async Task<ActionResult<AgendaDto>> Pot([FromBody] AgendaRequestDto agendaRequestDto)
        {
            var agenda = new AgendaDto { Id = Guid.NewGuid() };
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/{agenda.Id}", agenda);
        }
    }
}
