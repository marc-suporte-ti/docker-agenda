using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class ContatoRequestDtoToContatoEntity : Profile
    {
        public ContatoRequestDtoToContatoEntity()
        {
            CreateMap<ContatoRequestDto, ContatoEntity>()
                .ForMember(dest => dest.Nome, act => act.MapFrom(src => src.NomeContato))
                .ForMember(dest => dest.TipoContato, act => act.MapFrom(src => src.TipoContato))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
