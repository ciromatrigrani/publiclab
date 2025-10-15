using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class PostRequestSchemaSample : IExamplesProvider<PostRequest>
    {
        public PostRequest GetExamples() => new PostRequest
        {
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            Title = "This is the Title for my Amazing Post!"
        };
    }
}
