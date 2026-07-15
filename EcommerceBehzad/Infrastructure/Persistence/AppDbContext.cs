using System.Collections.Generic;
using System.Reflection.Emit;
using EcommerceBehzad.Domain.Entities;
using EcommerceBehzad.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBehzad.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BaseProduct> Products => Set<BaseProduct>();
        public DbSet<NintendoGame> Games => Set<NintendoGame>();
        public DbSet<ComicBook> Comics => Set<ComicBook>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Category Self-Referencing relationship
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(c => c.ParentCategory)
                      .WithMany(c => c.SubCategories)
                      .HasForeignKey(c => c.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // TPH Product Configuration
            modelBuilder.Entity<BaseProduct>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId);

                entity.HasDiscriminator<string>("ProductType")
                      .HasValue<NintendoGame>("Game")
                      .HasValue<ComicBook>("Comic");
            });

            modelBuilder.Entity<NintendoGame>(entity =>
            {
                entity.Property(g => g.Platform).HasMaxLength(50);
                entity.Property(g => g.DigitalKey).HasMaxLength(500);
            });

            modelBuilder.Entity<ComicBook>(entity =>
            {
                entity.Property(c => c.Author).HasMaxLength(150);
                entity.Property(c => c.Illustrator).HasMaxLength(150);
                entity.Property(c => c.Publisher).HasMaxLength(150);
                entity.Property(c => c.MongoFileId).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(256);
                entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Price).HasPrecision(18, 2);
                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId);
            });
        }
    }
}
