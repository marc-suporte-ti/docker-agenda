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
                //.ForMember(dest => dest.ItensContato, act => act.MapFrom(src => src.ItensContato))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
