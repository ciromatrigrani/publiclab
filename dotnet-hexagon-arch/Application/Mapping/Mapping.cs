using AutoMapper;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Model;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Mapping
{
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
}

