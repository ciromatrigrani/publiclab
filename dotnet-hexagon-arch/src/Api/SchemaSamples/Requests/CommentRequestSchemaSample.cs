using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class CommentRequestSchemaSample : IExamplesProvider<CommentRequest>
    {
        public CommentRequest GetExamples() => new CommentRequest
        {
            Author = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            PostId = System.Guid.NewGuid()
        };
    }
}
