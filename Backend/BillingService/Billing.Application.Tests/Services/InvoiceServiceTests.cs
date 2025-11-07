using AutoMapper;
using Billing.Application.Clients;
using Billing.Application.DTOs;
using Billing.Application.Services;
using Billing.Domain.Interfaces;
using Billing.Domain.Models;
using Billing.Domain.Models.Enums;
using Moq;
using Xunit;

namespace Billing.Application.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStockClient> _mockStockClient;
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _mockRepository = new Mock<IInvoiceRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockStockClient = new Mock<IStockClient>();

            _service = new InvoiceService(
               _mockRepository.Object,
               _mockMapper.Object,
               _mockStockClient.Object
           );
        }

        [Fact]
        public async Task PrintInvoiceAsync_InvoiceNotFound_ThrowsException()
        {
            _mockRepository.Setup(r => r.GetInvoiceWithItemsAsync(It.IsAny<int>()))
                           .ReturnsAsync((Invoice?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.PrintInvoiceAsync(1));
            Assert.Equal("Nota fiscal 1 não encontrada.", ex.Message);
        }

        [Fact]
        public async Task PrintInvoiceAsync_InvoiceNotOpen_ThrowsException()
        {
            var invoice = new Invoice { Id = 1, Status = InvoiceStatus.Fechada };
            _mockRepository.Setup(r => r.GetInvoiceWithItemsAsync(invoice.Id))
                           .ReturnsAsync(invoice);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.PrintInvoiceAsync(invoice.Id));
            Assert.Equal("Somente notas com status 'Aberta' podem ser impressas.", ex.Message);
        }

        [Fact]
        public async Task PrintInvoiceAsync_StockUpdateFails_ThrowsException()
        {
            var invoice = new Invoice
            {
                Id = 1,
                Status = InvoiceStatus.Aberta,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductCode = "PROD1", Quantity = 2 } }
            };

            _mockRepository.Setup(r => r.GetInvoiceWithItemsAsync(invoice.Id)).ReturnsAsync(invoice);
            _mockStockClient.Setup(c => c.UpdateStockAsync("PROD1", -2)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.PrintInvoiceAsync(invoice.Id));
            Assert.Equal("Não foi possível atualizar o saldo do produto PROD1", ex.Message);
        }

        [Fact]
        public async Task PrintInvoiceAsync_SuccessfullyClosesInvoice()
        {
            var invoice = new Invoice
            {
                Id = 1,
                Status = InvoiceStatus.Aberta,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductCode = "PROD1", Quantity = 1 } }
            };
            var updatedInvoice = new Invoice { Id = 1, Status = InvoiceStatus.Fechada };
            var dto = new InvoiceDTO { Id = 1, Status = "Fechada" };

            _mockRepository.Setup(r => r.GetInvoiceWithItemsAsync(invoice.Id)).ReturnsAsync(invoice);
            _mockStockClient.Setup(c => c.UpdateStockAsync("PROD1", -1)).ReturnsAsync(true);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Invoice>())).ReturnsAsync(updatedInvoice);
            _mockMapper.Setup(m => m.Map<InvoiceDTO>(updatedInvoice)).Returns(dto);

            var result = await _service.PrintInvoiceAsync(invoice.Id);

            Assert.Equal("Fechada", result.Status);
        }
    }
}
