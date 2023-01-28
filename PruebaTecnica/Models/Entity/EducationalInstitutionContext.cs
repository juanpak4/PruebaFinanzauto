using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models.Entity.Tables;

namespace PruebaTecnica.Models.Entity;

public partial class EducationalInstitutionContext : DbContext
{
    public EducationalInstitutionContext()
    {
    }
    public EducationalInstitutionContext(DbContextOptions<EducationalInstitutionContext> options): base(options)
    {
    }


    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
        
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.IdCourse);

            entity.ToTable("COURSE");

            entity.Property(e => e.Course1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Course");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.IdRating);

            entity.ToTable("RATING");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Rating1)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Rating");            
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.IdStudent);

            entity.ToTable("STUDENT");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.IdTeacher);

            entity.ToTable("TEACHER");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
