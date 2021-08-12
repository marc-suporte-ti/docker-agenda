﻿using System;

namespace DockerAgenda.Entity
{
    /// <summary>
    /// Registro do item do contato de um agenda cadastrada
    /// </summary>
    public class ItemContatoEntity
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

        /// <summary>
        /// Referência para o ID do contato
        /// </summary>
        public Guid ContatoEntityId { get; set; }

        /// <summary>
        /// Propriedade de nagaveção para o contato
        /// </summary>
        public ContatoEntity Contato { get; set; }
    }
}