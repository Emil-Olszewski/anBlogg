using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using AutoMapper;

namespace anBlogg.WebApi.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<Comment, ICommentOutputDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value));
        }
    }
}