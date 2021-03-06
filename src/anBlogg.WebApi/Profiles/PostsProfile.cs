﻿using anBlogg.Domain.Entities;
using anBlogg.Domain.ValueObjects;
using anBlogg.WebApi.Models;
using AutoMapper;
using System.Linq;

namespace anBlogg.WebApi.Profiles
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<PostInputDto, Post>()
                .ForMember(dest => dest.Title, opt =>
                    opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Contents, opt =>
                    opt.MapFrom(src => src.Contents))
                .ForMember(dest => dest.Tags, opt =>
                    opt.MapFrom(src => new Tags(src.Tags)))
                .ForAllOtherMembers(opt =>
                    opt.Ignore());

            CreateMap<Post, PostOutputDto>()
                .ForMember(dest => dest.Tags, opt =>
                    opt.MapFrom(src => src.Tags.Enumerate().ToArray()))
                .ForMember(dest => dest.Author, opt =>
                    opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Score, opt =>
                    opt.MapFrom(src => src.Score.Value))
                .ForMember(dest => dest.CommentsNumber, opt =>
                    opt.Ignore());

            CreateMap<Post, PostInputDto>()
                .ForMember(dest => dest.Tags, opt =>
                    opt.MapFrom(src => src.Tags.Enumerate().ToArray()));
        }
    }
}