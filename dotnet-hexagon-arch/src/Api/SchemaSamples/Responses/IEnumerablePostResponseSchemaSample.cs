using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class IEnumerablePostResponseSchemaSample : IExamplesProvider<IEnumerable<PostResponse>>
    {
        public IEnumerable<PostResponse> GetExamples() => new List<PostResponse>
        {
            new PostResponse {
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = System.DateTime.Now,
                Id = System.Guid.NewGuid(),
                Title = "This is the Title for my Amazing Post!"
             }
        };
    }
}
