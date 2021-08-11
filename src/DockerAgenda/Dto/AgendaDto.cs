using System;
using System.Collections.Generic;

namespace DockerAgenda.Dto
{
    public class AgendaDto
    {
        public Guid Id { get; set; }

        public IEnumerable<ContatoDto> Contatos { get; set; }
    }
}
