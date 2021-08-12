using AutoMapper;
using DockerAgenda.Dto;
using DockerAgenda.Entity;

namespace DockerAgenda.Mappings
{
    internal class AgendaRequestDtoToAgendaEntity : Profile
    {
        public AgendaRequestDtoToAgendaEntity()
        {
            CreateMap<AgendaRequestDto, AgendaEntity>()
                .ForMember(dest => dest.NomeResponsavel, act => act.MapFrom(src => src.NomeResponsavel))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
