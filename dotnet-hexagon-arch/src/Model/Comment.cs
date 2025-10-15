using System;
using System.ComponentModel.DataAnnotations;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Model
{
    public record Comment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
    }
}