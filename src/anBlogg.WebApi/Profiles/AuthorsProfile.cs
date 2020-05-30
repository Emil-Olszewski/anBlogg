using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using AutoMapper;

namespace anBlogg.WebApi.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, IAuthorShortOutputDto>()
                .ForMember(dest => dest.Score,
                opt => opt.MapFrom(src => src.Score.Value));

            CreateMap<Author, AuthorOutputDto>()
                .ForMember(dest => dest.Score, opt =>
                    opt.MapFrom(src => src.Score.Value))
                .ForMember(dest => dest.PostsNumber, opt =>
                    opt.Ignore())
                .ForMember(dest => dest.CommentsNumber, opt =>
                    opt.Ignore());
        }
    }
}