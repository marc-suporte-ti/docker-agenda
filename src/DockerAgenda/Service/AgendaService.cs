using AutoMapper;
using DockerAgenda.Data;
using DockerAgenda.Dto;
using DockerAgenda.Entity;
using DockerAgenda.Interfaces;
using System.Threading.Tasks;

namespace DockerAgenda.Service
{
    /// <summary>
    /// Implementação do contrato responsável por gerar o contrato do serviços da agenda
    /// </summary>
    public class AgendaService : IAgendaService
    {
        /// <summary>
        /// Contexto com DB
        /// </summary>
        private readonly DockerAgendaContext _contexto;

        /// <summary>
        /// Instância de mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor da implementação do contrato responsável por gerar o contrato do serviços da agenda
        /// </summary>
        /// <param name="contexto">Contexto com DB</param>
        /// <param name="mapper">Instância de mapper</param>
        public AgendaService(
            DockerAgendaContext contexto,
            IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }

        /// <summary>
        /// Insere novo item na agenda
        /// </summary>
        /// <param name="agendaRequestDto">Request com os dados para inclusão</param>
        /// <returns>Agenda cadastrada</returns>
        public async Task<AgendaDto> InserrirAdenga(AgendaRequestDto agendaRequestDto)
        {
            var agenda = _mapper.Map<AgendaEntity>(agendaRequestDto);
            await _contexto.Agendas.AddAsync(agenda);
            await _contexto.SaveChangesAsync();
            return _mapper.Map<AgendaDto>(agenda);
        }
    }
}
