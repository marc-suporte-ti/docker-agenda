using System;
using System.Collections.Generic;

namespace DockerAgenda.Entity
{
    /// <summary>
    /// Registro da agenda cadastrada
    /// </summary>
    public class AgendaEntity
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
        public IEnumerable<ContatoEntity> Contatos { get; set; }
    }
}
