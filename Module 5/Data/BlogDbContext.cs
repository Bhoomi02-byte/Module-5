﻿using Microsoft.EntityFrameworkCore;
using Module_5.Models.Entities;

namespace Module_5.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts) 
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasKey(l=> new {l.UserId, l.PostId});

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)  
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)  
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasKey(l => new { l.UserId, l.AuthorId });

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
               .HasOne(s => s.Author)
               .WithMany(u => u.Subscribers)
               .HasForeignKey(s => s.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);








        }
    }
}
