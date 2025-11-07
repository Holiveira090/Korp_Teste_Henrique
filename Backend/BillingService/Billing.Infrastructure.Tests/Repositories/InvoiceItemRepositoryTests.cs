using Billing.Domain.Models;
using Billing.Infrastructure.Context;
using Billing.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Billing.Infrastructure.Tests.Repositories
{
    public class InvoiceItemRepositoryTests
    {
        private BillingDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BillingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BillingDbContext(options);
        }

        [Fact]
        public async Task GetByInvoiceIdAsync_ReturnsItemsForGivenInvoice()
        {
            using var context = CreateContext();
            var repo = new InvoiceItemRepository(context);

            var items = new List<InvoiceItem>
            {
                new InvoiceItem { InvoiceId = 1, ProductCode = "P1", Quantity = 2 },
                new InvoiceItem { InvoiceId = 1, ProductCode = "P2", Quantity = 3 },
                new InvoiceItem { InvoiceId = 2, ProductCode = "P3", Quantity = 1 }
            };

            context.InvoiceItems.AddRange(items);
            await context.SaveChangesAsync();

            var result = await repo.GetByInvoiceIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(1, r.InvoiceId));
        }

        [Fact]
        public async Task GetByInvoiceIdAsync_ReturnsEmptyList_WhenNoItemsExist()
        {
            using var context = CreateContext();
            var repo = new InvoiceItemRepository(context);

            var result = await repo.GetByInvoiceIdAsync(99);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
