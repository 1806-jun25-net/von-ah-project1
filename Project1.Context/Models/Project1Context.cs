using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project1.Context.Models
{
    public partial class Project1Context : DbContext
    {
        public Project1Context()
        {
        }

        public Project1Context(DbContextOptions<Project1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<OrderPizza> OrderPizza { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizzas> Pizzas { get; set; }
        public virtual DbSet<Users> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<OrderPizza>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderPizza)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderPizz__Order__0D7A0286");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.OrderPizza)
                    .HasForeignKey(d => d.PizzaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderPizz__Pizza__0E6E26BF");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Location__0B91BA14");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__UserID__0C85DE4D");
            });

            modelBuilder.Entity<Pizzas>(entity =>
            {
                entity.HasKey(e => e.PizzaId);

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.Property(e => e.Price).HasColumnType("decimal(13, 2)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DefaultAddress)
                    .IsRequired()
                    .HasColumnName("defaultAddress")
                    .HasMaxLength(128);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ManagerFlag).HasDefaultValueSql("((0))");
            });
        }
    }
}
