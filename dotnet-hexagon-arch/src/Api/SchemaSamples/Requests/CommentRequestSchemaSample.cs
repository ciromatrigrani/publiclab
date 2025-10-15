using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class CommentRequestSchemaSample : IExamplesProvider<CommentRequest>
    {
        public CommentRequest GetExamples() => new CommentRequest
        {
            Author = "Ciro Matrigrani",
            Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
            PostId = System.Guid.NewGuid()
        };
    }
}
