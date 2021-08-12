using DockerAgenda.Dto;
using DockerAgenda.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DockerAgenda.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar contatos de uma agenda
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/agendas")]
    public class AgendasController : ControllerBase
    {
        /// <summary>
        /// Instância do serviço da api
        /// </summary>
        private readonly IAgendaService _agendaService;

        /// <summary>
        /// Construtor da controller responsável por gerenciar contatos de uma agenda
        /// </summary>
        /// <param name="agendaService">Instância do serviço da api</param>
        public AgendasController(IAgendaService agendaService)
        {
            _agendaService = agendaService;
        }

        /// <summary>
        /// Responsável por criar a agenda para o novo usuário
        /// </summary>
        /// <param name="agendaRequestDto">Dados para registro da agenda</param>
        /// <returns>Agenda criada</returns>
        [HttpPost]
        public async Task<ActionResult<AgendaDto>> PostAgenda([FromBody] AgendaRequestDto agendaRequestDto)
        {
            var agenda = await _agendaService.InserrirAdenga(agendaRequestDto);

            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/{agenda.Id}", agenda);
        }

        /// <summary>
        /// Consulta agenda pelo id
        /// </summary>
        /// <param name="id">Id do registro da agenda</param>
        /// <returns>Informações da agenda consultada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AgendaDto>> GetAgenda(Guid id)
        {
            var agenda = new AgendaDto { Id = Guid.NewGuid() };
            return Ok(agenda);
        }

        /// <summary>
        /// Cria o contato na agenda informada
        /// </summary>
        /// <param name="id">Id da agenda criada</param>
        /// <param name="contatoRequestDto">Dados do contato para cadastro</param>
        /// <returns>Contato criado na agenda</returns>
        [HttpPost("{id}/contatos")]
        public async Task<ActionResult<ContatoDto>> PostContato(Guid id, [FromBody] ContatoRequestDto contatoRequestDto)
        {
            var contato = new ContatoDto { Id = Guid.NewGuid() };
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/{id}/{contato.Id}", contato);
        }

        /// <summary>
        /// Consulta contato pelo id do contato e id da agenda
        /// </summary>
        /// <param name="id">Id da agenda onde o contato foi cadastrado</param>
        /// <param name="idContato">Id do contato para consulta</param>
        /// <returns>Contato encontrado</returns>
        [HttpGet("{id}/contatos/{idContato}")]
        public async Task<ActionResult<ContatoDto>> GetContato(Guid id, Guid idContato)
        {
            var contato = new ContatoDto { Id = Guid.NewGuid() };
            return Ok(contato);
        }

        [HttpPost("{id}/contatos/{idContato}")]
        public async Task<ActionResult<ItemContatoDto>> PostItemContato(Guid id, Guid idContato, [FromBody] ItemContatoRequestDto itemContatoRequestDto)
        {
            var contato = new ItemContatoDto { Id = Guid.NewGuid() };
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/{id}/{contato.Id}", contato);
        }
    }
}
