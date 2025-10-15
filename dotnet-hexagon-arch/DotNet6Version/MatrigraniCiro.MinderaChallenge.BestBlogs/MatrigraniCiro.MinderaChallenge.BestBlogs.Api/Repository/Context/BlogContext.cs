using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;

// this is used for our verification tests, don't rename or change the access modifier
public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Post> Posts { get; set; }
}
