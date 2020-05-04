using AutoMapper;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;

namespace anBlogg.WebApi.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, AuthorOutputDto>()
                .ForMember(dest => dest.NumberOfPosts,
                opt => opt.MapFrom(src => src.Posts.Count))
                .ForMember(dest => dest.NumberOfComments,
                opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.Score,
                opt => opt.MapFrom(src => src.Score.Value));
        }
    }
}
