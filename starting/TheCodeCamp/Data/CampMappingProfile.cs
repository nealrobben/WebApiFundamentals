using AutoMapper;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>().ForMember(x => x.Venue, opt => opt.MapFrom(x => x.Location.VenueName))
                .ReverseMap();

            CreateMap<Talk, TalkModel>().ReverseMap().ForMember(x => x.Speaker, opt => opt.Ignore())
                .ForMember(x => x.Camp, opt => opt.Ignore());

            CreateMap<Speaker, SpeakerModel>().ReverseMap();
        }
    }
}