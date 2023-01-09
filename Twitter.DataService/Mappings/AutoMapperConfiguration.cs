using AutoMapper;
using Twitter.Model.Entities;
using Twitter.Service.Dto;

namespace Twitter.Service.Mappings
{
    public class AutoMapperConfiguration : Profile
    {

        public AutoMapperConfiguration()
        {

            CreateMap<HashTag, HashTagDto>()
                .ForMember(dest => dest.TweeterTweetId, source => source.MapFrom(source => source.TweeterTweetId));
        }
    }
}
