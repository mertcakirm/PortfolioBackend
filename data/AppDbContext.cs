using System;
using System.Collections.Generic;
using ASPNetProject.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ASPNetProject.data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogContent> BlogContents { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=PortfolioDB;user=root;password=sananepagerl12", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Blogid).HasName("PRIMARY");

            entity.Property(e => e.BlogDescTr)
                .HasMaxLength(100)
                .HasColumnName("BLOG_desc_tr");
            entity.Property(e => e.BlogDescription)
                .HasMaxLength(70)
                .HasColumnName("Blog_description");
            entity.Property(e => e.BlogImageBase64).HasColumnName("Blog_image_base64");
            entity.Property(e => e.BlogName).HasMaxLength(100);
            entity.Property(e => e.BlogNameTr)
                .HasMaxLength(50)
                .HasColumnName("BLOG_Name_tr");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("curdate()");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ShowBlog).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<BlogContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Blog_Contents");

            entity.HasIndex(e => e.Blogid, "fk_Blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentEn)
                .HasColumnType("text")
                .HasColumnName("content_en");
            entity.Property(e => e.ContentTr)
                .HasColumnType("text")
                .HasColumnName("content_tr");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageBase64).HasColumnName("image_base64");
            entity.Property(e => e.TitleEn)
                .HasMaxLength(255)
                .HasColumnName("title_en");
            entity.Property(e => e.TitleTr)
                .HasMaxLength(255)
                .HasColumnName("title_tr");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogContents)
                .HasForeignKey(d => d.Blogid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_Blog");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EducationText).HasMaxLength(100);
            entity.Property(e => e.Egitim).HasMaxLength(100);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Homepage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DescriptionEn)
                .HasColumnType("text")
                .HasColumnName("description_en");
            entity.Property(e => e.DescriptionTr)
                .HasColumnType("text")
                .HasColumnName("description_tr");
            entity.Property(e => e.HeaderEn)
                .HasMaxLength(255)
                .HasColumnName("header_en");
            entity.Property(e => e.HeaderTr)
                .HasMaxLength(255)
                .HasColumnName("header_tr");
            entity.Property(e => e.MainImageBase64).HasColumnName("main_image_base64");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DescriptionEn)
                .HasColumnType("text")
                .HasColumnName("description_en");
            entity.Property(e => e.DescriptionTr)
                .HasColumnType("text")
                .HasColumnName("description_tr");
            entity.Property(e => e.Href)
                .HasMaxLength(255)
                .HasColumnName("href");
            entity.Property(e => e.ImageBase64).HasColumnName("image_base64");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
            entity.Property(e => e.TitleEn)
                .HasMaxLength(255)
                .HasColumnName("title_en");
            entity.Property(e => e.TitleTr)
                .HasMaxLength(255)
                .HasColumnName("title_tr");
            entity.Property(e => e.UsedSkills)
                .HasMaxLength(100)
                .HasColumnName("Used_skills");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PRIMARY");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
            entity.Property(e => e.RoleName).HasMaxLength(30);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Proficiency).HasColumnName("proficiency");
            entity.Property(e => e.SkillName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.Roleid, "fk_Role");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(30);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
