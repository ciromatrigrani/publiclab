using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class CommentResponseSchemaSample : IExamplesProvider<CommentResponse>
    {
        public CommentResponse GetExamples() => new CommentResponse
        {
            Author = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = System.DateTime.Now,
            PostId = System.Guid.NewGuid(),
            Id = System.Guid.NewGuid()
        };
    }
}
