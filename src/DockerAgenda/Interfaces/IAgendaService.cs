using DockerAgenda.Dto;
using System;
using System.Threading.Tasks;

namespace DockerAgenda.Interfaces
{
    /// <summary>
    /// Interface responsável por gerar o contrato do serviços da agenda
    /// </summary>
    public interface IAgendaService
    {
        /// <summary>
        /// Insere novo item na agenda
        /// </summary>
        /// <param name="agendaRequestDto">Request com os dados para inclusão</param>
        /// <returns>Agenda cadastrada</returns>
        Task<AgendaDto> InserirAgendaAsync(AgendaRequestDto agendaRequestDto);

        /// <summary>
        /// Pesquisa a agenda solicitada
        /// </summary>
        /// <param name="id">Id da agenda</param>
        /// <returns>Agenda encontrada</returns>
        Task<AgendaDto> ConsultarAgendaAsync(Guid id);

        /// <summary>
        /// Insere contato na agenda informada
        /// </summary>
        /// <param name="idAgenda">Id da agenda para inclusão</param>
        /// <param name="contatoRequestDto">Dados do contato para inclusão</param>
        /// <returns>Contato cadastrado</returns>
        Task<ContatoDto> InserirContatoAsync(Guid idAgenda, ContatoRequestDto contatoRequestDto);

        /// <summary>
        /// Pesquisa o contato da agenda
        /// </summary>
        /// <param name="idAgenda">Id da agenda para pesquisa</param>
        /// <param name="idContato">Id do contato</param>
        /// <returns>Dados do contato pesquisado</returns>
        Task<ContatoDto> ConsultarContatoAsync(Guid idAgenda, Guid idContato);

        /// <summary>
        /// Insere item no contato
        /// </summary>
        /// <param name="idAgenda">Id da agenda para validação</param>
        /// <param name="idContato">Id do contato para vallidação</param>
        /// <param name="itemContatoRequestDto">Dados do item do contato para inclusão</param>
        /// <returns>Item incluído no contato</returns>
        Task<ItemContatoDto> InserirItemContatoAsync(Guid idAgenda, Guid idContato, ItemContatoRequestDto itemContatoRequestDto);
    }
}
