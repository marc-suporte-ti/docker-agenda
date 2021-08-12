using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class ItemContatoEntityToItemContatoDto : Profile
    {
        public ItemContatoEntityToItemContatoDto()
        {
            CreateMap<ItemContatoEntity, ItemContatoDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Registro, act => act.MapFrom(src => src.Registro))
                .ForMember(dest => dest.TipoItemContato, act => act.MapFrom(src => src.TipoItemContato))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
