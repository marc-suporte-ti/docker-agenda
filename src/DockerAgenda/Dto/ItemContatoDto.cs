using System;

namespace DockerAgenda.Dto
{
    /// <summary>
    /// Registro do item do contato de um agenda cadastrada
    /// </summary>
    public class ItemContatoDto
    {
        /// <summary>
        /// Id identificador da agenda cadastrada
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Dados para registrar do contato
        /// </summary>
        public string Registro { get; set; }

        /// <summary>
        /// Define o tipo do item do contato
        /// </summary>
        public TipoItemContato TipoItemContato { get; set; }
    }
}
