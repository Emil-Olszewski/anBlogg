using AutoMapper;
using anBlogg.Domain.Entities;
using anBlogg.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace anBlogg.WebApi.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<Comment, CommentOutputDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value));
        }
    }
}
