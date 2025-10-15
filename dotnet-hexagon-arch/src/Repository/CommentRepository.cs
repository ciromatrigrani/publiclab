using MatrigraniCiro.HexagonArch.BestBlogs.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MatrigraniCiro.HexagonArch.BestBlogs.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogContext _context;

        public CommentRepository(BlogContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Comment>> GetAll(CancellationToken token)
        {
            return await _context.Comments.ToListAsync(token);
        }

        public async Task<Comment> Get(Guid id, CancellationToken token)
        {
            return await _context.Comments.Where(c => c.Id == id).FirstAsync(token);
        }

        public async Task<Comment> Create(Comment comment, CancellationToken token)
        {
            _context.Comments.Add(comment);
            await ((DbContext)_context).SaveChangesAsync(token);
            return await Get(comment.Id, token);
        }

        public async Task<Comment> Update(Comment comment, CancellationToken token)
        {
            try
            {
                await _context.Comments.Where(c => c.Id == comment.Id).FirstAsync(token);
                _context.Comments.Update(comment);
            }
            catch (Exception ex)
            {
                _context.Comments.Add(comment);
                throw new Exception(comment.Id.ToString(), ex);
            }
            finally
            {
                await ((DbContext)_context).SaveChangesAsync(token);
            }
            return comment;

        }

        public async Task<bool> Delete(Guid id, CancellationToken token)
        {
            _context.Comments.Remove(new Comment { Id = id });
            return (await ((DbContext)_context).SaveChangesAsync(token) > 0);
        }

        public async Task<IEnumerable<Comment>> GetByPostId(Guid postId, CancellationToken token)
        {
            return await _context.Comments.Where(c => c.Id.Equals(postId)).ToListAsync(token);
        }
    }
}