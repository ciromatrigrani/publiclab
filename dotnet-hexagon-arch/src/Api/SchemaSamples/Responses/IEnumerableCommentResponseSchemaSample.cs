using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class IEnumerableCommentResponseSchemaSample : IExamplesProvider<IEnumerable<CommentResponse>>
    {
        public IEnumerable<CommentResponse> GetExamples() => new List<CommentResponse>
        {
            new CommentResponse {
                Author = "Ciro Matrigrani",
                Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
                CreationDate = System.DateTime.Now,
                PostId = System.Guid.NewGuid(),
                Id = System.Guid.NewGuid()
             }
        };
    }
}
