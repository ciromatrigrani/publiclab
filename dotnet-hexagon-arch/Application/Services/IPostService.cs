using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponse>> GetAll(CancellationToken token = default);
        Task<PostResponse> Get(Guid postId, CancellationToken token = default);
        Task<PostResponse> Post(Guid newPostId, PostRequest postRequest, CancellationToken token = default);
        Task<PostResponse> Patch(Guid postId, JsonPatchDocument<PostRequest> postPatchRequest, CancellationToken token = default);
        Task<PostResponse> Put(Guid postId, PostRequest postRequest, CancellationToken token = default);
        Task<bool> Delete(Guid postId, CancellationToken token = default);
    }
}
