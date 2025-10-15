using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class PostResponseSchemaSample : IExamplesProvider<PostResponse>
    {
        public PostResponse GetExamples() => new PostResponse
        {
            Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
            CreationDate = System.DateTime.Now,
            Id = System.Guid.NewGuid(),
            Title = "This is the Title for my Amazing Post!"
        };
    }
}
