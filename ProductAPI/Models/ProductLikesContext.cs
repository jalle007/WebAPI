using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductAPI.Models {
  public partial class ProductLikesContext : DbContext {
    public virtual DbSet<Image> Image { get; set; }
    public virtual DbSet<Like> Like { get; set; }
    public virtual DbSet<Platform> Platform { get; set; }
    public virtual DbSet<Product> Product { get; set; }

    public ProductLikesContext (DbContextOptions<ProductLikesContext> options) : base(options) { }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
    //    //optionsBuilder.UseSqlServer(@"Data Source=mylaptop;Initial Catalog=ProductLikes;Integrated Security=True");
    //}

    protected override void OnModelCreating (ModelBuilder modelBuilder) {
      modelBuilder.Entity<Image>(entity => {
        entity.Property(e => e.ImageId).HasColumnName("ImageID");

        entity.Property(e => e.Description).HasColumnType("varchar(1000)");
        entity.Property(e => e.Username).HasColumnType("varchar(255)");
        entity.Property(e => e.ProfilePicUrl).HasColumnType("varchar(1000)");

        entity.Property(e => e.DeviceId)
            .HasColumnName("DeviceID")
            .HasColumnType("varchar(255)");

        entity.Property(e => e.DeviceType).HasColumnType("varchar(255)");

        entity.Property(e => e.Picture)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        entity.Property(e => e.ProductId).HasColumnName("ProductID");

        entity.Property(e => e.Title).HasColumnType("varchar(1000)");

        entity.Property(e => e.UserId)
            .HasColumnName("UserID")
            .HasColumnType("varchar(255)");

        entity.Property(e => e.Timestamp)
           .HasColumnType("datetime")
           .HasDefaultValueSql("getdate()");

        entity.HasOne(d => d.Product)
            .WithMany(p => p.Image)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("FK_Image_Product");
      });

      modelBuilder.Entity<Like>(entity => {
        entity.Property(e => e.LikeId).HasColumnName("LikeID");

        entity.Property(e => e.ImageId).HasColumnName("ImageID");

        entity.Property(e => e.PlatformId).HasColumnName("PlatformID");

        entity.Property(e => e.Timestamp)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()");

        entity.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("UserID")
            .HasColumnType("varchar(50)");

        entity.HasOne(d => d.Image)
            .WithMany(p => p.Like)
            .HasForeignKey(d => d.ImageId)
            .HasConstraintName("FK_Like_Image");

        entity.HasOne(d => d.Platform)
            .WithMany(p => p.Like)
            .HasForeignKey(d => d.PlatformId)
            .HasConstraintName("FK_Like_Platform");
      });

      modelBuilder.Entity<Platform>(entity => {
        entity.Property(e => e.PlatformId).HasColumnName("PlatformID");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");
      });

      modelBuilder.Entity<Product>(entity => {
        entity.Property(e => e.ProductId).HasColumnName("ProductID");

        entity.Property(e => e.SKU)
                .HasColumnName("SKU")
                .IsRequired();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");
      });
      }
    }
  }