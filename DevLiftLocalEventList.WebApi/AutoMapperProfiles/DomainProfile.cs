using AutoMapper;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.WebApi.Dto;

namespace DevLiftLocalEventList.WebApi.AutoMapperProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<EventType, EventTypeDto>().ReverseMap();
        }
    }
}