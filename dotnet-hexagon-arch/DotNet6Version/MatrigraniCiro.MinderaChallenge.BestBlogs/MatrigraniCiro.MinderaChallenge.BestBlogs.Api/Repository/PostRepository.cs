using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;

public class PostRepository : IPostRepository
{
    private readonly BlogContext _context;

    public PostRepository(BlogContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<Post>> GetAll(CancellationToken token)
    {
        return await _context.Posts.ToListAsync(token);
    }

    public async Task<Post> Get(Guid id, CancellationToken token)
    {
        return await _context.Posts.Where(c => c.Id == id).FirstAsync(token);
    }

    public async Task<Post> Create(Post post, CancellationToken token)
    {
        _context.Posts.Add(post);
        await ((DbContext)_context).SaveChangesAsync(token);
        return await Get(post.Id, token);
    }

    public async Task<Post> Update(Post post, CancellationToken token)
    {
        try
        {
            await _context.Posts.Where(c => c.Id == post.Id).FirstAsync(token);
            _context.Posts.Update(post);
        }
        catch (Exception ex)
        {
            _context.Posts.Add(post);
            throw new Exception(post.Id.ToString(), ex);
        }
        finally
        {
            await ((DbContext)_context).SaveChangesAsync(token);
        }
        return post;

    }

    public async Task<bool> Delete(Guid id, CancellationToken token)
    {
        _context.Posts.Remove(new Post { Id = id });
        return (await ((DbContext)_context).SaveChangesAsync(token) > 0);
    }
}
