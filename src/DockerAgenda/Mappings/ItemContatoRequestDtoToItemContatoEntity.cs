using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class ItemContatoRequestDtoToItemContatoEntity : Profile
    {
        public ItemContatoRequestDtoToItemContatoEntity()
        {
            CreateMap<ItemContatoRequestDto, ItemContatoEntity>()
                .ForMember(dest => dest.Registro, act => act.MapFrom(src => src.Registro))
                .ForMember(dest => dest.Observacao, act => act.MapFrom(src => src.Observacao))
                .ForMember(dest => dest.TipoItemContato, act => act.MapFrom(src => src.TipoItemContato))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
