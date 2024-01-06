using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Set> Sets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Category
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Title).IsRequired();

            // Configure Comment
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.PublishDate).IsRequired();
            modelBuilder.Entity<Comment>().Property(c => c.Content).IsRequired();
            modelBuilder.Entity<Comment>().HasOne(c => c.Publisher).WithMany();

            // Configure Conversation
            modelBuilder.Entity<Conversation>().HasKey(c => c.Id);

            // Configure Exercise
            modelBuilder.Entity<Exercise>().HasKey(e => e.Id);
            modelBuilder.Entity<Exercise>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Exercise>().HasOne(e => e.Category).WithMany();

            // Configure Message
            modelBuilder.Entity<Message>().HasKey(m => m.Id);
            modelBuilder.Entity<Message>().Property(m => m.Content).IsRequired();
            modelBuilder.Entity<Message>().Property(m => m.DateSent).IsRequired();
            modelBuilder.Entity<Message>().HasOne(m => m.Sender).WithMany();

            // Configure Post
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Post>().Property(p => p.Title).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.Description).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.Content).IsRequired();
            modelBuilder.Entity<Post>().HasOne(p => p.Publisher).WithMany();
            modelBuilder.Entity<Post>().HasOne(p => p.Category).WithMany();

            // Configure Profile
            modelBuilder.Entity<Profile>().HasKey(p => p.Id);
            modelBuilder.Entity<Profile>().Property(p => p.Username).IsRequired();
            modelBuilder.Entity<Profile>().Property(p => p.Age).IsRequired();
            modelBuilder.Entity<Profile>().Property(p => p.Email).IsRequired();

            // Configure Session
            modelBuilder.Entity<Session>().HasKey(s => s.Id);
            modelBuilder.Entity<Session>().Property(s => s.Name).IsRequired();
            modelBuilder.Entity<Session>().Property(s => s.Date).IsRequired();
            modelBuilder.Entity<Session>().Property(s => s.Duration).IsRequired();

            // Configure Set
            modelBuilder.Entity<Set>().HasKey(s => s.Id);
            modelBuilder.Entity<Set>().Property(s => s.Reps).IsRequired();
            modelBuilder.Entity<Set>().Property(s => s.Rest).IsRequired();
            modelBuilder.Entity<Set>().Property(s => s.Mode).IsRequired();

            // Relationships
            modelBuilder.Entity<Comment>().HasMany(c => c.Answers).WithOne();
            modelBuilder.Entity<Conversation>().HasMany(c => c.Profiles);
            modelBuilder.Entity<Conversation>().HasMany(c => c.Messages).WithOne();
            modelBuilder.Entity<Session>().HasMany(s => s.Exercises).WithOne();
        }

    }
}
