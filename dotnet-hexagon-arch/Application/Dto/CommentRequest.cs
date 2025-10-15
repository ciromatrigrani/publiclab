using System;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto
{
    public record CommentRequest
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}