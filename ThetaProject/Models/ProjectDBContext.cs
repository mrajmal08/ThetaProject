using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ThetaProject.Models
{
    public partial class ProjectDBContext : DbContext
    {
        public ProjectDBContext()
        {
        }

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<LoginUser> LoginUser { get; set; }
        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=LAPTOP-82GAEQ6T\\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;User ID=sa;Password=ajmal");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Cv)
                    .HasColumnName("CV")
                    .HasMaxLength(250);

                entity.Property(e => e.Dept).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.DOB).HasMaxLength(50);

                entity.Property(e => e.IsCreatedBy)
                .HasColumnName("Is_Created_By")
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.Dept).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<LoginUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FName)
                 .HasColumnName("F_Name")
                 .HasMaxLength(50);

                entity.Property(e => e.LName)
                 .HasColumnName("L_Name")
                 .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.DOB).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

               
            });
        }
    }
}
