using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Mapping;

public class Mapping : Profile
{
    public Mapping()
    {
        this.CreateMap<CommentRequest, CommentResponse>().ReverseMap();
        this.CreateMap<Comment, CommentResponse>().ReverseMap();
        this.CreateMap<Comment, CommentRequest>().ReverseMap();
        this.CreateMap<PostRequest, PostResponse>().ReverseMap();
        this.CreateMap<Post, PostResponse>().ReverseMap();
        this.CreateMap<Post, PostRequest>().ReverseMap();
    }
}

