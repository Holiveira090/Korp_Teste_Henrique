using Microsoft.EntityFrameworkCore;
using Stock.Domain.Interfaces;
using Stock.Domain.Models;
using Stock.Infrastructure.Context;

namespace Stock.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(StockDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Code == code);
        }
        public async Task<bool> UpdateStockAsync(string productCode, int quantityChange)
        {
            var product = await _dbSet.FirstOrDefaultAsync(p => p.Code == productCode);
            if (product == null)
                return false;

            product.StockQuantity += quantityChange;

            if (product.StockQuantity < 0)
                throw new InvalidOperationException("Saldo insuficiente.");

            _dbSet.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
