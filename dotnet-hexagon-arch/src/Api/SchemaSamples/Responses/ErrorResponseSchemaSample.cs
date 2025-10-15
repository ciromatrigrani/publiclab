using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class ErrorResponseSchemaSample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            Message = "Here will appear some message about the error."
        };
    }
}
