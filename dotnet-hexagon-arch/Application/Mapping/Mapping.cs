using AutoMapper;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using MatrigraniCiro.HexagonArch.BestBlogs.Model;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Mapping
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

