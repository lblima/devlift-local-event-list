using AutoMapper;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.WebApi.ViewModels;

namespace DevLiftLocalEventList.WebApi.AutoMapperProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Event, EventViewModel>().ReverseMap();
            CreateMap<EventType, EventTypeViewModel>().ReverseMap();
        }
    }
}