using anBlogg.Application.Services.Models;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using AutoMapper;

namespace anBlogg.WebApi.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<Comment, ICommentOutputDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value));

            CreateMap<Comment, CommentOutputDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value));
        }
    }
}