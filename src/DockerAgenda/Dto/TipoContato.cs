namespace DockerAgenda.Dto
{
    /// <summary>
    /// Define as possibilidades da classificação deste contato
    /// </summary>
    public enum TipoContato
    {
        /// <summary>
        /// Quando contato é um parente
        /// </summary>
        Parente = 1,
        /// <summary>
        /// Quando contato é um amigo
        /// </summary>
        Amigo = 2,
        /// <summary>
        /// Quando contato é um colega
        /// </summary>
        Colega = 3,
        /// <summary>
        /// Quando contato é um desconhecido
        /// </summary>
        Desconhecido = 99
    }
}
