using Stock.Application.DTOs;
using Stock.Domain.Models;

namespace Stock.Application.Services.Interfaces
{
    public interface IProductServices : IServicesBase<ProductDto>
    {
        Task<ProductDto?> GetByCodeAsync(string code);
        Task<bool> UpdateStockAsync(string productCode, int quantityChange);
    }
}
