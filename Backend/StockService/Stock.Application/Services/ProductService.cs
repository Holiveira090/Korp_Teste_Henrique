using AutoMapper;
using Stock.Application.DTOs;
using Stock.Application.Services.Interfaces;
using Stock.Domain.Interfaces;
using Stock.Domain.Models;

namespace Stock.Application.Services
{
    public class ProductService : ServicesBase<Product, ProductDto>, IProductServices
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IMapper mapper) : base(productRepository, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> GetByCodeAsync(string code)
        {
            var product = await _productRepository.GetByCodeAsync(code);
            return product is null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateStockAsync(string productCode, int quantityChange)
        {
            try
            {
                return await _productRepository.UpdateStockAsync(productCode, quantityChange);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar saldo: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha inesperada ao atualizar saldo: {ex.Message}");
            }
        }
    }
}
