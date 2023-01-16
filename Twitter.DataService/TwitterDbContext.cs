using Microsoft.EntityFrameworkCore;
using Twitter.Model.Entities;

namespace Twitter.Data
{
    public class TwitterDbContext : DbContext
    {

        public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Author.OnModelCreating(modelBuilder.Entity<Author>());
            Tweet.OnModelCreating(modelBuilder.Entity<Tweet>());
            HashTag.OnModelCreating(modelBuilder.Entity<HashTag>());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author>? AuthorEntities { get; set; }

        public DbSet<Tweet>? TweetEntities { get; set; }

        public DbSet<HashTag>? HashTagsEntities { get; set; }

    }


}
