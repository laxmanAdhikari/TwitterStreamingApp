using Microsoft.EntityFrameworkCore;
using Twitter.Core.Data;
using Twitter.Model.Entities;

namespace Twitter.BlazorServer.Data
{
    public class TwitterDbContext : DbContext
    {
        public DbSet<Tweet>? TweetEntities { get; set; }

        public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Author.OnModelCreating(builder.Entity<Author>());
            Tweet.OnModelCreating(builder.Entity<Tweet>());
        }
    }


}
