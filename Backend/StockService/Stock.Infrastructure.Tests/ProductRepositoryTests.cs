using Microsoft.EntityFrameworkCore;
using Stock.Domain.Models;
using Stock.Infrastructure.Context;
using Stock.Infrastructure.Repositories;
using Xunit;

namespace Stock.Infrastructure.Tests
{
    public class ProductRepositoryTests
    {
        private StockDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new StockDbContext(options);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnProduct_WhenCodeExists()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var product = new Product { Code = "P123", Description = "Produto Teste", StockQuantity = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await repo.GetByCodeAsync("P123");

            Assert.NotNull(result);
            Assert.Equal("Produto Teste", result!.Description);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenCodeDoesNotExist()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var result = await repo.GetByCodeAsync("INVALID");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldIncreaseStock_WhenPositiveQuantity()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var product = new Product { Code = "PX1", Description = "Produto X", StockQuantity = 5 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await repo.UpdateStockAsync("PX1", 3);

            var updated = await context.Products.FirstAsync(p => p.Code == "PX1");
            Assert.True(result);
            Assert.Equal(8, updated.StockQuantity);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldDecreaseStock_WhenNegativeQuantity()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var product = new Product { Code = "PX2", Description = "Produto Y", StockQuantity = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await repo.UpdateStockAsync("PX2", -4);

            var updated = await context.Products.FirstAsync(p => p.Code == "PX2");
            Assert.True(result);
            Assert.Equal(6, updated.StockQuantity);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldThrow_WhenStockGoesNegative()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var product = new Product { Code = "PX3", Description = "Produto Z", StockQuantity = 2 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateStockAsync("PX3", -5));
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldReturnFalse_WhenProductNotFound()
        {
            using var context = CreateContext();
            var repo = new ProductRepository(context);

            var result = await repo.UpdateStockAsync("INVALID", 5);

            Assert.False(result);
        }
    }
}
