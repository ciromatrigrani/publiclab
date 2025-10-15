using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Filters;


namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.SchemaSamples.Responses
{
    public class JsonPatchDocumentCommentRequestSchemaSample : IExamplesProvider<JsonPatchDocument<CommentRequest>>
    {
        public JsonPatchDocument<CommentRequest> GetExamples()
        {
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "x", path: "/y", null, value: "z"));
            return patchEntity;
        }
    }
}
