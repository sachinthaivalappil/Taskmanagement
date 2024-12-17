using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task_app.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminMaster> AdminMasters { get; set; }

    public virtual DbSet<TaskMaster> TaskMasters { get; set; }

    public virtual DbSet<TaskProgress> TaskProgresses { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=sachints;Password=passgit rm -r --cached .artifacts/\n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminMaster>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("admin_master_pkey");

            entity.ToTable("admin_master");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });

        modelBuilder.Entity<TaskMaster>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("task_master_pkey");

            entity.ToTable("task_master");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");
            entity.Property(e => e.AssignedTo).HasColumnName("assigned_to");
            entity.Property(e => e.DateOfAssignment).HasColumnName("date_of_assignment");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.TaskDcr)
                .HasMaxLength(100)
                .HasColumnName("task_dcr");
            entity.Property(e => e.TaskName)
                .HasMaxLength(100)
                .HasColumnName("task_name");
            entity.Property(e => e.TaskProgress).HasColumnName("task_progress");
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(50)
                .HasColumnName("task_status");
            entity.Property(e => e.TaskTargetdate).HasColumnName("task_targetdate");
        });

        modelBuilder.Entity<TaskProgress>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.SubTaskId, e.ProgressDatetime }).HasName("task_progress_pkey");

            entity.ToTable("task_progress");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.SubTaskId).HasColumnName("sub_task_id");
            entity.Property(e => e.ProgressDatetime).HasColumnName("progress_datetime");
            entity.Property(e => e.PercentageOfCompletion)
                .HasPrecision(5, 2)
                .HasColumnName("percentage_of_completion");
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_master_pkey");

            entity.ToTable("user_master");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });
        modelBuilder.HasSequence("user_master_user_id_seq").StartsAt(100000L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
