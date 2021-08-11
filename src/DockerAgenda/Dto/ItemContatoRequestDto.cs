namespace DockerAgenda.Dto
{
    /// <summary>
    /// Registro a intenção de cadastro de um item no contato de uma agenda
    /// </summary>
    public class ItemContatoRequestDto
    {
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
