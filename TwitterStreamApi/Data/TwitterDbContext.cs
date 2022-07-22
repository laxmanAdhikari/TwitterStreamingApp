﻿using Microsoft.EntityFrameworkCore;
using Twitter.Model.Entities;
using TwitterStreamApi.Dto;

namespace TwitterStreamApi.Data
{
    public class TwitterDbContext : DbContext
    {

        public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            Author.OnModelCreating(modelBuilder.Entity<Author>());
            Tweet.OnModelCreating(modelBuilder.Entity<Tweet>());

            base.OnModelCreating (modelBuilder);
        }

        public DbSet<Author> AuthorEntities { get; set; }

        public DbSet<Tweet> TweetEntities { get; set; }

    }


}
