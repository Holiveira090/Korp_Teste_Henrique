using Billing.Domain.Models;
using Billing.Infrastructure.Context;
using Billing.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Billing.Infrastructure.Tests.Repositories
{
    public class InvoiceRepositoryTests
    {
        private BillingDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BillingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BillingDbContext(options);
        }

        [Fact]
        public async Task GetBySequentialNumberAsync_ReturnsInvoiceWithItems()
        {
            using var context = CreateContext();
            var repo = new InvoiceRepository(context);

            var invoice = new Invoice
            {
                Id = 1,
                SequentialNumber = 100,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductCode = "P1", Quantity = 2 } }
            };

            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var result = await repo.GetBySequentialNumberAsync(100);

            Assert.NotNull(result);
            Assert.Single(result!.Items);
            Assert.Equal("P1", result.Items.First().ProductCode);
        }

        [Fact]
        public async Task GetNextSequentialNumberAsync_ReturnsIncrementedValue()
        {
            using var context = CreateContext();
            var repo = new InvoiceRepository(context);

            context.Invoices.AddRange(
                new Invoice { SequentialNumber = 1 },
                new Invoice { SequentialNumber = 5 }
            );
            await context.SaveChangesAsync();

            var next = await repo.GetNextSequentialNumberAsync();

            Assert.Equal(6, next);
        }

        [Fact]
        public async Task GetInvoiceWithItemsAsync_ReturnsInvoiceWithRelatedItems()
        {
            using var context = CreateContext();
            var repo = new InvoiceRepository(context);

            var invoice = new Invoice
            {
                Id = 1,
                SequentialNumber = 10,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductCode = "PX", Quantity = 3 } }
            };

            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var result = await repo.GetInvoiceWithItemsAsync(1);

            Assert.NotNull(result);
            Assert.Single(result!.Items);
            Assert.Equal("PX", result.Items.First().ProductCode);
        }
    }
}
