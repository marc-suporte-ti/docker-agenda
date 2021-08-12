using AutoMapper;
using DockerAgenda.Data;
using DockerAgenda.Dto;
using DockerAgenda.Entity;
using DockerAgenda.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<AgendaDto> InserirAgendaAsync(AgendaRequestDto agendaRequestDto)
        {
            var agenda = _mapper.Map<AgendaEntity>(agendaRequestDto);
            await _contexto.Agendas.AddAsync(agenda);
            await _contexto.SaveChangesAsync();
            return _mapper.Map<AgendaDto>(agenda);
        }

        /// <summary>
        /// Pesquisa a agenda solicitada
        /// </summary>
        /// <param name="id">Id da agenda</param>
        /// <returns>Agenda encontrada</returns>
        public async Task<AgendaDto> ConsultarAgendaAsync(Guid id)
        {
            var agenda = await _contexto.Agendas.FindAsync(id);
            return _mapper.Map<AgendaDto>(agenda);
        }

        /// <summary>
        /// Insere contato na agenda informada
        /// </summary>
        /// <param name="contatoRequestDto">Dados do contato para inclusão</param>
        /// <returns>Contato cadastrado</returns>
        public async Task<ContatoDto> InserirContatoAsync(Guid idAgenda, ContatoRequestDto contatoRequestDto)
        {
            var agenda = await _contexto.Agendas.FindAsync(idAgenda);
            var contato = _mapper.Map<ContatoEntity>(contatoRequestDto);
            contato.Agenda = agenda;
            contato.AgendaId = contato.Id;
            await _contexto.Contatos.AddAsync(contato);
            await _contexto.SaveChangesAsync();
            return _mapper.Map<ContatoDto>(contato);
        }

        /// <summary>
        /// Pesquisa o contato da agenda
        /// </summary>
        /// <param name="idAgenda">Id da agenda para pesquisa</param>
        /// <param name="idContato">Id do contato</param>
        /// <returns>Dados do contato pesquisado</returns>
        public async Task<ContatoDto> ConsultarContatoAsync(Guid idAgenda, Guid idContato)
        {
            var contato = await _contexto.Contatos.FirstOrDefaultAsync(contato => contato.AgendaId == idAgenda && contato.Id == idContato);
            return _mapper.Map<ContatoDto>(contato);
        }
    }
}
