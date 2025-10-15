using MatrigraniCiro.HexagonArch.BestBlogs.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Repository
{
    public interface IPostRepository
    {
        Task<Post> Create(Post post, CancellationToken token);
        Task<bool> Delete(Guid id, CancellationToken token);
        Task<Post> Get(Guid id, CancellationToken token);
        Task<IEnumerable<Post>> GetAll(CancellationToken token);
        Task<Post> Update(Post post, CancellationToken token);
    }
}