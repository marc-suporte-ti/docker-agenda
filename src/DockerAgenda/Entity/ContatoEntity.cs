using System;
using System.Collections.Generic;

namespace DockerAgenda.Entity
{
    /// <summary>
    /// Registro do contato cadastrado na agenda
    /// </summary>
    public class ContatoEntity
    {
        /// <summary>
        /// Construtor do registro contato cadastrado na agenda
        /// </summary>
        public ContatoEntity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Id identificador do contato cadastrado
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do contato
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Relacionamento com este contato
        /// </summary>
        public TipoContato TipoContato { get; set; }

        /// <summary>
        /// Lista de itens do contato
        /// </summary>
        public IEnumerable<ItemContatoEntity> ItensContato { get; set; }

        /// <summary>
        /// Referência para o ID da agenda
        /// </summary>
        public Guid AgendaId { get; set; }

        /// <summary>
        /// Propriedade de nagaveção para a agenda
        /// </summary>
        public AgendaEntity Agenda { get; set; }
    }
}
