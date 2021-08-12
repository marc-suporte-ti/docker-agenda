using System;
using System.Collections.Generic;

namespace DockerAgenda.Dto
{
    /// <summary>
    /// Registro da agenda cadastrada
    /// </summary>
    public class AgendaDto
    {
        /// <summary>
        /// Id identificador da agenda cadastrada
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do responsável pela agenda
        /// </summary>
        public string NomeResponsavel { get; set; }

        /// <summary>
        /// Lista de contatos da agenda
        /// </summary>
        public IEnumerable<ContatoDto> Contatos { get; set; }
    }
}
