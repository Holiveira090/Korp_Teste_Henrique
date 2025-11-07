using AutoMapper;
using Moq;
using Stock.Application.DTOs;
using Stock.Application.Services;
using Stock.Domain.Interfaces;
using Stock.Domain.Models;
using Xunit;

namespace Stock.Application.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnProductDto_WhenProductExists()
        {
            var product = new Product { Id = 1, Code = "P001", Description = "Produto Teste", StockQuantity = 10 };
            var productDto = new ProductDto { Id = 1, Code = "P001", Description = "Produto Teste", StockQuantity = 10 };

            _productRepositoryMock.Setup(r => r.GetByCodeAsync("P001"))
                .ReturnsAsync(product);

            _mapperMock.Setup(m => m.Map<ProductDto>(product))
                .Returns(productDto);

            var result = await _productService.GetByCodeAsync("P001");

            Assert.NotNull(result);
            Assert.Equal("P001", result.Code);
            Assert.Equal("Produto Teste", result.Description);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            _productRepositoryMock.Setup(r => r.GetByCodeAsync("P999"))
                .ReturnsAsync((Product?)null);

            var result = await _productService.GetByCodeAsync("P999");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            _productRepositoryMock.Setup(r => r.UpdateStockAsync("P001", 5))
                .ReturnsAsync(true);

            var result = await _productService.UpdateStockAsync("P001", 5);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldThrowInvalidOperationException_WhenInsufficientStock()
        {
            _productRepositoryMock.Setup(r => r.UpdateStockAsync("P001", -100))
                .ThrowsAsync(new InvalidOperationException("Saldo insuficiente."));

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _productService.UpdateStockAsync("P001", -100));

            Assert.Contains("Saldo insuficiente", ex.Message);
        }

        [Fact]
        public async Task UpdateStockAsync_ShouldThrowGenericException_WhenUnexpectedErrorOccurs()
        {
            _productRepositoryMock.Setup(r => r.UpdateStockAsync("P001", 10))
                .ThrowsAsync(new Exception("Erro no banco."));

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _productService.UpdateStockAsync("P001", 10));

            Assert.Contains("Falha inesperada", ex.Message);
        }
    }
}
