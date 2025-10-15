using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;

public interface IPostRepository
{
    Task<Post> Create(Post post, CancellationToken token);
    Task<bool> Delete(Guid id, CancellationToken token);
    Task<Post> Get(Guid id, CancellationToken token);
    Task<IEnumerable<Post>> GetAll(CancellationToken token);
    Task<Post> Update(Post post, CancellationToken token);
}
