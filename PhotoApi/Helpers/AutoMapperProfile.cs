using AutoMapper;
using PhotoApi.Dtos.Photo;
using PhotoApi.entities;

namespace PhotoApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PhotoDto, PhotoEntity>();
            CreateMap<PhotoCreateDto, PhotoEntity>();
        }
    }
}

