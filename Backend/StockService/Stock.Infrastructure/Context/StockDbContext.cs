using Microsoft.EntityFrameworkCore;
using Stock.Domain.Models;

namespace Stock.Infrastructure.Context
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) 
        {}

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Code)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("code")
                      .IsRequired();
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(255)
                      .HasColumnName("description")
                      .IsRequired();

                entity.Property(e => e.StockQuantity)
                      .IsRequired()
                      .HasColumnType("integer")
                      .HasColumnName("stock_quantity");
            });
        }
    }
}
