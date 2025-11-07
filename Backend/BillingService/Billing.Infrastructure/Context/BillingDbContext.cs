using Billing.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Context
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options)
            : base(options)
        {}

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoices");

                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id)
                      .HasColumnName("id");

                entity.Property(i => i.SequentialNumber)
                      .HasColumnName("number")
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(i => i.Status)
                      .HasColumnName("status")
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(i => i.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(i => i.SequentialNumber)
                      .IsUnique();
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("invoice_items");

                entity.HasKey(ii => ii.Id);

                entity.Property(ii => ii.Id)
                      .HasColumnName("id");

                entity.Property(ii => ii.ProductCode)
                      .HasColumnName("product_code")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(ii => ii.Quantity)
                      .HasColumnName("quantity")
                      .IsRequired();

                entity.Property(ii => ii.InvoiceId)
                      .HasColumnName("invoice_id")
                      .IsRequired();

                entity.HasOne(ii => ii.Invoice)
                      .WithMany(i => i.Items)
                      .HasForeignKey(ii => ii.InvoiceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
