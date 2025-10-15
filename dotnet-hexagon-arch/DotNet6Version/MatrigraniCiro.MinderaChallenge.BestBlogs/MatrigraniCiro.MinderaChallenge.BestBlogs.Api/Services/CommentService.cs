using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Exceptions;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services.Helpers;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository commentRepository;
    private readonly IMapper mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        this.commentRepository = commentRepository;
        this.mapper = mapper;
    }

    public async Task<bool> Delete(Guid commentId, CancellationToken token = default)
    {
        return await this.commentRepository.Delete(commentId, token);
    }

    public async Task<CommentResponse> Get(Guid commentId, CancellationToken token = default)
    {
        return mapper.Map<CommentResponse>(await this.commentRepository.Get(commentId, token)); ;
    }

    public async Task<IEnumerable<CommentResponse>> GetAll(CancellationToken token = default)
    {
        return mapper.Map<IEnumerable<CommentResponse>>(await this.commentRepository.GetAll(token)); ;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsByPostId(Guid postId, CancellationToken token)
    {
        var comments = await this.commentRepository.GetByPostId(postId, token);
        var commentDtos = mapper.Map<IEnumerable<CommentResponse>>(comments);

        return commentDtos;
    }

    public async Task<CommentResponse> Patch(Guid commentId, JsonPatchDocument<CommentRequest> commentPatchRequest, CancellationToken token = default)
    {
        try
        {
            var comment = await this.commentRepository.Get(commentId, token);
            var commentRequest = mapper.Map<CommentRequest>(comment);
            commentPatchRequest.ApplyTo(commentRequest);

            Guard.ValidateComment(commentRequest);

            comment = mapper.Map<Comment>(commentRequest);
            comment.Id = commentId;
            comment = await this.commentRepository.Update(comment, token);
            return mapper.Map<CommentResponse>(comment);
        }
        catch (JsonPatchException ex) { throw new BadRequestException(commentPatchRequest, ex); }
    }

    public async Task<CommentResponse> Post(Guid newCommentId, CommentRequest commentRequest, CancellationToken token = default)
    {
        Guard.ValidateComment(commentRequest);

        var comment = mapper.Map<Comment>(commentRequest);
        comment.Id = newCommentId;
        return mapper.Map<CommentResponse>(await this.commentRepository.Create(comment, token));
    }

    public async Task<CommentResponse> Put(Guid commentId, CommentRequest commentRequest, CancellationToken token = default)
    {
        Guard.ValidateComment(commentRequest);

        var comment = mapper.Map<Comment>(commentRequest);
        comment.Id = commentId;
        return mapper.Map<CommentResponse>(await this.commentRepository.Update(comment, token));
    }
}
