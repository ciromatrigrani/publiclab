using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class IEnumerablePostResponseSchemaSample : IExamplesProvider<IEnumerable<PostResponse>>
    {
        public IEnumerable<PostResponse> GetExamples() => new List<PostResponse>
        {
            new PostResponse {
                Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
                CreationDate = System.DateTime.Now,
                Id = System.Guid.NewGuid(),
                Title = "This is the Title for my Amazing Post!"
             }
        };
    }
}
