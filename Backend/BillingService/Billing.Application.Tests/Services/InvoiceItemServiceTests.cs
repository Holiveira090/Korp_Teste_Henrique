using AutoMapper;
using Billing.Application.Clients;
using Billing.Application.DTOs;
using Billing.Domain.Interfaces;
using Billing.Domain.Models;
using Moq;
using Xunit;

namespace Billing.Application.Tests.Services
{
    public class InvoiceItemServiceTests
    {
        private readonly Mock<IInvoiceItemRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStockClient> _mockStockClient;
        private readonly InvoiceItemService _service;

        public InvoiceItemServiceTests()
        {
            _mockRepository = new Mock<IInvoiceItemRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockStockClient = new Mock<IStockClient>();

            _service = new InvoiceItemService(_mockRepository.Object, _mockMapper.Object, _mockStockClient.Object);
        }

        [Fact]
        public async Task CreateAsync_ProductDoesNotExist_ThrowsException()
        {
            var dto = new InvoiceItemDto { ProductCode = "INVALID", Quantity = 1 };
            _mockStockClient.Setup(x => x.CheckProductExistsAsync(dto.ProductCode))
                            .ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));
            Assert.Equal($"Produto com código {dto.ProductCode} não existe no estoque.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_QuantityExceedsStock_ThrowsException()
        {
            var dto = new InvoiceItemDto { ProductCode = "PROD1", Quantity = 5 };
            _mockStockClient.Setup(x => x.CheckProductExistsAsync(dto.ProductCode))
                            .ReturnsAsync(true);
            _mockStockClient.Setup(x => x.GetStockQuantityAsync(dto.ProductCode))
                            .ReturnsAsync(3);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));
            Assert.Equal($"Não é possível adicionar {dto.Quantity} unidades. Apenas 3 disponíveis no estoque.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidProduct_AddsItemSuccessfully()
        {
            var dto = new InvoiceItemDto { ProductCode = "PROD1", Quantity = 2 };
            var entity = new InvoiceItem { ProductCode = "PROD1", Quantity = 2 };

            _mockStockClient.Setup(x => x.CheckProductExistsAsync(dto.ProductCode))
                            .ReturnsAsync(true);
            _mockStockClient.Setup(x => x.GetStockQuantityAsync(dto.ProductCode))
                            .ReturnsAsync(5);

            _mockMapper.Setup(m => m.Map<InvoiceItem>(dto)).Returns(entity);
            _mockMapper.Setup(m => m.Map<InvoiceItemDto>(entity)).Returns(dto);

            _mockRepository.Setup(x => x.AddAsync(entity)).ReturnsAsync(entity);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(dto.ProductCode, result.ProductCode);
            Assert.Equal(dto.Quantity, result.Quantity);
        }
    }
}
