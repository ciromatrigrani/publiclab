using System;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto
{
    public record PostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}