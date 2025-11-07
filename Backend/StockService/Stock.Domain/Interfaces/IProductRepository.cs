using Stock.Domain.Models;

namespace Stock.Domain.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product?> GetByCodeAsync(string code);
        Task<bool> UpdateStockAsync(string productCode, int quantityChange);
    }
}
