using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class ContatoEntityToContatoDto : Profile
    {
        public ContatoEntityToContatoDto()
        {
            CreateMap<ContatoEntity, ContatoDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, act => act.MapFrom(src => src.Nome))
                .ForMember(dest => dest.TipoContato, act => act.MapFrom(src => src.TipoContato))
                .ForMember(dest => dest.ItensContato, act => act.MapFrom(src => src.ItensContato))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
