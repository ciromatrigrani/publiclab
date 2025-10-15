using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class PostRequestSchemaSample : IExamplesProvider<PostRequest>
    {
        public PostRequest GetExamples() => new PostRequest
        {
            Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
            Title = "This is the Title for my Amazing Post!"
        };
    }
}
