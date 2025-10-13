using Microsoft.EntityFrameworkCore;
using MyBlogAdminService.Models;

namespace MyBlogAdminService.Data
{
    public class MyBlogAdminDbContext : DbContext
    {
        public MyBlogAdminDbContext(DbContextOptions<MyBlogAdminDbContext> Options) : base(Options)
        { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
