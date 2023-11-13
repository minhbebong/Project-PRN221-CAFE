using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace CafeShopFPT.Models
{
    public partial class QuanLyQuanCafeContext : DbContext
    {
        public QuanLyQuanCafeContext()
        {
        }

        public QuanLyQuanCafeContext(DbContextOptions<QuanLyQuanCafeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BillInfo> BillInfos { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TableFood> TableFoods { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",optional: false,reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DbConStr"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.UserName, "IX_Account");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'New User')");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Phone)
                    .HasMaxLength(13)
                    .IsFixedLength();

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DateCheckIn).HasColumnType("datetime");

                entity.Property(e => e.DateCheckOut).HasColumnType("datetime");

                entity.Property(e => e.TableId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Bill__AccountId__4E88ABD4");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.TableId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Bill_TableFood");
            });

            modelBuilder.Entity<BillInfo>(entity =>
            {
                entity.ToTable("BillInfo");

                entity.Property(e => e.BillId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FoodId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.BillInfos)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillInfo_Food");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");

                entity.Property(e => e.FoodId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FoodName).HasMaxLength(100);

                entity.Property(e => e.ImgPath).HasColumnType("text");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Food_Category");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TableFood>(entity =>
            {
                entity.HasKey(e => e.TableId)
                    .HasName("PK__TableFoo__7D5F01EE8BA19403");

                entity.ToTable("TableFood");

                entity.Property(e => e.TableId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
