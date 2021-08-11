namespace DockerAgenda.Dto
{
    /// <summary>
    /// Registro a intenção de cadastro de um contato em uma agenda
    /// </summary>
    public class ContatoRequestDto
    {
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
