using Microsoft.EntityFrameworkCore;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Data
{
    public class NewsContext :  DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options)
            :base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SubTopic> SubTopics { get; set; }
        public DbSet<Topic> Topica { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ArticleAuthor> ArticleAuthors { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleAuthor>()
                .HasKey(aa => new { aa.ArticleId, aa.AuthorId});

            modelBuilder.Seed(new MainInitializer());
        }
        
    }
}
