using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetAll(CancellationToken token = default);
    Task<CommentResponse> Get(Guid commentId, CancellationToken token = default);
    Task<CommentResponse> Post(Guid newCommentId, CommentRequest commentRequest, CancellationToken token = default);
    Task<bool> Delete(Guid commentId, CancellationToken token = default);
    Task<CommentResponse> Patch(Guid commentId, JsonPatchDocument<CommentRequest> commentPatchRequest, CancellationToken token = default);
    Task<CommentResponse> Put(Guid commentId, CommentRequest commentRequest, CancellationToken token = default);
    Task<IEnumerable<CommentResponse>> GetCommentsByPostId(Guid postId, CancellationToken token);
}
