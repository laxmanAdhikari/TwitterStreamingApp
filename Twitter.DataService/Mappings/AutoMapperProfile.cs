using AutoMapper;
using Twitter.Model.Entities;
using Twitter.Service.Dto;

namespace Twitter.Service.Mappings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<HashTag, HashTagDto>();
                
        }
    }
}