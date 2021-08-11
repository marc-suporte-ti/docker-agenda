using System;

namespace DockerAgenda.Dto
{
    /// <summary>
    /// Registro do contato cadastrado na agenda
    /// </summary>
    public class ContatoDto
    {
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
    }
}
