using System;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto
{
    public record CommentResponse
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
    }
}