using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;

public interface ICommentRepository
{
    Task<Comment> Create(Comment comment, CancellationToken token);
    Task<bool> Delete(Guid id, CancellationToken token);
    Task<Comment> Get(Guid id, CancellationToken token);
    Task<IEnumerable<Comment>> GetAll(CancellationToken token);
    Task<IEnumerable<Comment>> GetByPostId(Guid postId, CancellationToken token);
    Task<Comment> Update(Comment comment, CancellationToken token);
}
