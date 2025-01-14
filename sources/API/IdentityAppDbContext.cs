﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dommain.Entities;
using Model;
using Domain.Entities;

namespace API
{
    public class IdentityAppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<ConversationMembersEntity> ConversationMembers { get; set; }
        public DbSet<ExerciseEntity> Exercises { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<LikedPostEntity> LikedPosts { get; set; }
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<SetEntity> Sets { get; set; }
        public DbSet<PracticalExerciseEntity> PracticalExercises { get; set; }
        public DbSet<ExerciseInPostEntity> ExerciseInPost { get; set; }

        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>().HasIndex(u => u.Title).IsUnique();
            modelBuilder.Entity<ProfileEntity>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<LikedPostEntity>()
                .HasIndex(l => new { l.LikedByThisProfileId, l.LikedPostId })
                .IsUnique();

            modelBuilder.Entity<ConversationMembersEntity>()
                .HasIndex(c => new { c.ConversationId, c.ProfileId })
                .IsUnique();
            
            modelBuilder.Entity<ExerciseInPostEntity>()
                .HasIndex(p => new { p.ExerciseId, p.PostId })
                .IsUnique();
            // TODO A COMPLETER
        }
    }
}
