using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.DTOs;
using Stock.Application.Services.Interfaces;
using Stock.Controller.Controllers;
using Xunit;

namespace Stock.Controller.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductServices> _productServiceMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductServices>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_productServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByCode_ShouldReturnBadRequest_WhenCodeIsEmpty()
        {
            var result = await _controller.GetByCode("");

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Código inválido.", badRequest.Value);
        }

        [Fact]
        public async Task GetByCode_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _productServiceMock.Setup(s => s.GetByCodeAsync("X123")).ReturnsAsync((ProductDto?)null);

            var result = await _controller.GetByCode("X123");

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto com código 'X123' não encontrado.", notFound.Value);
        }

        [Fact]
        public async Task GetByCode_ShouldReturnOk_WhenProductExists()
        {
            var product = new ProductDto { Id = 1, Code = "A1", Description = "Produto A" };
            _productServiceMock.Setup(s => s.GetByCodeAsync("A1")).ReturnsAsync(product);

            var result = await _controller.GetByCode("A1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal("A1", value.Code);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnBadRequest_WhenProductCodeIsEmpty()
        {
            var result = await _controller.UpdateStock("", new StockUpdateDto { QuantityChange = 5 });

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Código do produto inválido.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            var result = await _controller.UpdateStock("A1", null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Dados inválidos.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnBadRequest_WhenQuantityIsZero()
        {
            var result = await _controller.UpdateStock("A1", new StockUpdateDto { QuantityChange = 0 });

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("QuantityChange deve ser diferente de zero.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _productServiceMock.Setup(s => s.GetByCodeAsync("A1")).ReturnsAsync((ProductDto?)null);

            var result = await _controller.UpdateStock("A1", new StockUpdateDto { QuantityChange = 10 });

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto com código 'A1' não encontrado.", notFound.Value);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnOk_WhenStockIsUpdatedSuccessfully()
        {
            var existingProduct = new ProductDto { Id = 1, Code = "A1", Description = "Produto A", StockQuantity = 10 };
            var updatedProduct = new ProductDto { Id = 1, Code = "A1", Description = "Produto A", StockQuantity = 15 };

            _productServiceMock.SetupSequence(s => s.GetByCodeAsync("A1"))
                .ReturnsAsync(existingProduct)
                .ReturnsAsync(updatedProduct);

            _productServiceMock.Setup(s => s.UpdateStockAsync("A1", 5)).ReturnsAsync(true);

            var result = await _controller.UpdateStock("A1", new StockUpdateDto { QuantityChange = 5 });

            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(15, product.StockQuantity);
        }

        [Fact]
        public async Task UpdateStock_ShouldReturnBadRequest_WhenUpdateFails()
        {
            var product = new ProductDto { Id = 1, Code = "A1", Description = "Produto A", StockQuantity = 10 };

            _productServiceMock.Setup(s => s.GetByCodeAsync("A1")).ReturnsAsync(product);
            _productServiceMock.Setup(s => s.UpdateStockAsync("A1", 5)).ReturnsAsync(false);

            var result = await _controller.UpdateStock("A1", new StockUpdateDto { QuantityChange = 5 });

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Não foi possível atualizar o saldo.", badRequest.Value);
        }
    }
}
