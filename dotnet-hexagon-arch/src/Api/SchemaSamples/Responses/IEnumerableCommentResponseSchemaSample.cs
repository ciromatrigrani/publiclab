using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class IEnumerableCommentResponseSchemaSample : IExamplesProvider<IEnumerable<CommentResponse>>
    {
        public IEnumerable<CommentResponse> GetExamples() => new List<CommentResponse>
        {
            new CommentResponse {
                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = System.DateTime.Now,
                PostId = System.Guid.NewGuid(),
                Id = System.Guid.NewGuid()
             }
        };
    }
}
