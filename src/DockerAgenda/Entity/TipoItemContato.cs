namespace DockerAgenda.Entity
{
    /// <summary>
    /// Define as possibilidades da classificação deste contato
    /// </summary>
    public enum TipoItemContato
    {
        /// <summary>
        /// Quando tipo do contato é um telefone
        /// </summary>
        Telefone = 1,
        /// <summary>
        /// Quando tipo do contato é um email
        /// </summary>
        Email = 2,
        /// <summary>
        /// Quando tipo do contato é um fax
        /// </summary>
        Fax = 3,
        /// <summary>
        /// Quando tipo do contato é um outros
        /// </summary>
        Outros = 99
    }
}
