using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dommain.Entities;

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
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<SetEntity> Sets { get; set; }

        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
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
                        modelBuilder.Entity<Conversation>().Property(c => c.Name).IsRequired();
                        modelBuilder.Entity<Conversation>().Property(c => c.Picture).IsRequired(false);

                        // Configure ConversationMembers
                        modelBuilder.Entity<ConversationMembers>().HasKey(c => c.Id);
                        modelBuilder.Entity<ConversationMembers>().Property(c => c.ConversationId).IsRequired();
                        modelBuilder.Entity<ConversationMembers>().Property(c => c.ProfileId).IsRequired();
                        modelBuilder.Entity<ConversationMembers>()
                            .HasIndex(cm => new { cm.ConversationId, cm.ProfileId })
                            .IsUnique();

                        // Configure Exercise
                        modelBuilder.Entity<Exercise>().HasKey(e => e.Id);
                        modelBuilder.Entity<Exercise>().Property(e => e.Name).IsRequired();
                        modelBuilder.Entity<Exercise>().HasOne(e => e.Category).WithMany();

                        // Configure Message
                        modelBuilder.Entity<Message>().HasKey(m => m.Id);
                        modelBuilder.Entity<Message>().Property(m => m.Content).IsRequired();
                        modelBuilder.Entity<Message>().Property(m => m.DateSent).IsRequired();

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
                        modelBuilder.Entity<Profile>().Property(p => p.Picture).IsRequired(false);
                        modelBuilder.Entity<Profile>().HasIndex(p => p.Email).IsUnique();
                        modelBuilder.Entity<Profile>().HasIndex(p => p.Username).IsUnique();

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
                        modelBuilder.Entity<Set>(entity =>
                        {
                            entity.HasKey(e => e.Id);
                            entity.HasOne<Exercise>()
                                  .WithMany()
                                  .HasForeignKey(e => e.ExerciseId)
                                  .OnDelete(DeleteBehavior.Cascade);

                            entity.Ignore(e => e.Weight);
                            entity.Property(e => e.WeightJson)
                                  .HasColumnName("Weight")
                                  .IsRequired();
                        });
                        modelBuilder.Entity<Exercise>(entity =>
                        {
                            entity.HasKey(e => e.Id);
                            entity.HasOne<Session>()
                                  .WithMany()
                                  .HasForeignKey(e => e.SessionId)
                                  .OnDelete(DeleteBehavior.Cascade);
                        });
                        modelBuilder.Entity<Session>(entity =>
                        {
                            entity.HasKey(e => e.Id);
                            entity.HasOne(e => e.Profile)
                                  .WithMany()
                                  .OnDelete(DeleteBehavior.Cascade);
                        });

                        modelBuilder.Entity<Message>()
                        .HasOne<Conversation>()
                        .WithMany()
                        .HasForeignKey(m => m.ConversationId)
                        .IsRequired();

                        modelBuilder.Entity<Message>()
                            .HasOne(m => m.Sender)
                            .WithMany()
                            .HasForeignKey("SenderId")
                            .IsRequired();*/
        }
    }
}
