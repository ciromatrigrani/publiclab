using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Exceptions;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class ExceedMaxSizeCharactersExceptionSchemaSample : IExamplesProvider<ExceedMaxSizeCharactersException>
    {
        public ExceedMaxSizeCharactersException GetExamples() => new ExceedMaxSizeCharactersException(
            new Tuple<string, int, string>("Author", 30, "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani")
            , null
         );
    }
}
