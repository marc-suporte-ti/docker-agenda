using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class AgendaEntityToAgendaDto : Profile
    {
        public AgendaEntityToAgendaDto()
        {
            CreateMap<AgendaEntity, AgendaDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.NomeResponsavel, act => act.MapFrom(src => src.NomeResponsavel))
                .ForMember(dest => dest.Contatos, act => act.MapFrom(src => src.Contatos))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
