using AutoMapper;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;

namespace anBlogg.WebApi.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, AuthorShortOutputDto>()
                .ForMember(dest => dest.Score,
                opt => opt.MapFrom(src => src.Score.Value));

            CreateMap<Author, AuthorOutputDto>()
                .ForMember(dest => dest.Score, opt =>
                    opt.MapFrom(src => src.Score.Value));
        }
    }
}
