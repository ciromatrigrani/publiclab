using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class PostResponseSchemaSample : IExamplesProvider<PostResponse>
    {
        public PostResponse GetExamples() => new PostResponse
        {
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = System.DateTime.Now,
            Id = System.Guid.NewGuid(),
            Title = "This is the Title for my Amazing Post!"
        };
    }
}
