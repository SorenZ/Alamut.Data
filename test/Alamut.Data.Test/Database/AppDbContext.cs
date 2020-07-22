using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.Test.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        { }

        public AppDbContext(DbContextOptions options) : base(options)
        { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Story>()
                .HasKey(k =>
                    new
                    {
                        k.Id, k.Key
                    });
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Story> Stories { get; set; }
    }

}