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
        Task<AgendaDto> InserirAgenda(AgendaRequestDto agendaRequestDto);

        /// <summary>
        /// Pesquisa a agenda solicitada
        /// </summary>
        /// <param name="id">Id da agenda</param>
        /// <returns>Agenda encontrada</returns>
        Task<AgendaDto> ConsultarAgenda(Guid id);
    }
}
