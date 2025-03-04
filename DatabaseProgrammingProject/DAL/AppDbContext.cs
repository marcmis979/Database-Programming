using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<BasketPosition> BasketPositions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPosition> OrderPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<BasketPosition>()
            .HasKey(bp => new { bp.ProductID, bp.UserID });

            modelBuilder.Entity<OrderPosition>()
            .HasKey(op => new { op.OrderID, op.ProductID });

            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => ug.ID);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Group)
            .WithMany(g => g.Products)
            .HasForeignKey(p => p.GroupID)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductGroup>()
            .HasOne(pg => pg.ParentGroup)
            .WithMany(pg => pg.ChildGroups)
            .HasForeignKey(pg => pg.ParentID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BasketPosition>()
            .HasOne(bp => bp.Product)
            .WithMany(b => b.BasketPositions)
            .HasForeignKey(bp => bp.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BasketPosition>()
            .HasOne(bp => bp.User)
            .WithMany(b => b.BasketPositions)
            .HasForeignKey(bp => bp.UserID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPosition>()
            .HasOne(op => op.Product)
            .WithMany(o => o.OrderPositions)
            .HasForeignKey(op => op.ProductID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderPosition>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderPositions)
            .HasForeignKey(op => op.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(ug => ug.Users)
                .HasForeignKey(ug => ug.GroupID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserGroup>()
                .HasMany(ug => ug.Users)
                .WithOne(u => u.Group)
                .HasForeignKey(ug => ug.GroupID)
                .OnDelete(DeleteBehavior.SetNull);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Shop;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
