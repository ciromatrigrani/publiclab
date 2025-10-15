using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class CommentResponseSchemaSample : IExamplesProvider<CommentResponse>
    {
        public CommentResponse GetExamples() => new CommentResponse
        {
            Author = "Ciro Matrigrani",
            Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
            CreationDate = System.DateTime.Now,
            PostId = System.Guid.NewGuid(),
            Id = System.Guid.NewGuid()
        };
    }
}
