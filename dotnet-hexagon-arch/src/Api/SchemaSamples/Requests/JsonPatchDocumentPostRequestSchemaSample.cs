using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Filters;


namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.SchemaSamples.Responses
{
    public class JsonPatchDocumentPostRequestSchemaSample : IExamplesProvider<JsonPatchDocument<PostRequest>>
    {
        public JsonPatchDocument<PostRequest> GetExamples()
        {
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "x", path: "/y", null, value: "z"));
            return patchEntity;
        }
    }
}
