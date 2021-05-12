using System;
using Evaluation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Evaluation.Brokers.Context
{
    public partial class EvaluationContext : DbContext , IStorageBroker
    {
        public EvaluationContext()
        {
        }

        public EvaluationContext(DbContextOptions<EvaluationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Evaluatee> Evaluatees { get; set; }
        public virtual DbSet<EvaluateeEventQuestion> EvaluateeEventQuestions { get; set; }
        public virtual DbSet<Evaluator> Evaluators { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventQuestion> EventQuestions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EvaluationDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Evaluatee>(entity =>
            {
                entity.ToTable("Evaluatees", "Evaluation");

                entity.HasOne(d => d.Evaluator)
                    .WithMany(p => p.Evaluatees)
                    .HasForeignKey(d => d.EvaluatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evaluatees_Evaluators");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Evaluatees)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evaluatees_Personels");
            });

            modelBuilder.Entity<EvaluateeEventQuestion>(entity =>
            {
                entity.ToTable("EvaluateeEventQuestions", "Evaluation");

                entity.Property(e => e.DescriptivAnswer).HasMaxLength(4000);

                entity.HasOne(d => d.Evaluatee)
                    .WithMany(p => p.EvaluateeEventQuestions)
                    .HasForeignKey(d => d.EvaluateeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluateeEventQuestions_Evaluatees");

                entity.HasOne(d => d.EventQuestion)
                    .WithMany(p => p.EvaluateeEventQuestions)
                    .HasForeignKey(d => d.EventQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluateeEventQuestions_EventQuestions");
            });

            modelBuilder.Entity<Evaluator>(entity =>
            {
                entity.ToTable("Evaluators", "Evaluation");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Evaluators)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evaluators_Events");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Evaluators)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evaluators_Personels");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events", "Event");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.EntryDatetime).HasColumnType("datetime");

                entity.Property(e => e.EventTitle)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.NextStartDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventQuestion>(entity =>
            {
                entity.ToTable("EventQuestions", "Event");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventQuestions)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventQuestions_Events");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.EventQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventQuestions_Questions");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions", "Event");

                entity.Property(e => e.QuestionTitle)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.HasOne(d => d.QuestionGroup)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventQuestions_EventQuestionGroups");
            });

            modelBuilder.Entity<QuestionGroup>(entity =>
            {
                entity.ToTable("QuestionGroups", "Event");

                entity.Property(e => e.GroupTitle)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "Evaluation");

                entity.Property(e => e.Email).HasMaxLength(80);

                entity.Property(e => e.Password).HasMaxLength(4000);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.SureName)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
