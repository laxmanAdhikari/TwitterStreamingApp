using AutoMapper;
using Twitter.Model;
using TwitterStreamApi.Dto;

namespace TwitterStreamApi.Mappings
{
    public class MappingProfiles : Profile
    {
       public MappingProfiles()
        {
            //CreateMap<Tweet, TwitterDto>()
            //    .ForMember(dest => dest.Id, source => source.MapFrom(source => source.Id))
            //    .ForMember(dest => dest.TwitterPublished, source => source.MapFrom(source => source.Created))
            //    .ForMember(dest => dest.AuthorId, source => source.MapFrom(source => source.AuthorId))
            //    .ForMember(dest => dest.Content, source => source.MapFrom(source => source.Text));
        }
    }
}
