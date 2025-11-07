using Billing.Application.DTOs;
using Billing.Application.Services.Interfaces;
using Billing.Controller.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Billing.Controller.Tests.Controllers
{
    public class InvoiceItemControllerTests
    {
        private readonly Mock<IInvoiceItemService> _mockService;
        private readonly Mock<ILogger<InvoiceItemController>> _mockLogger;
        private readonly InvoiceItemController _controller;

        public InvoiceItemControllerTests()
        {
            _mockService = new Mock<IInvoiceItemService>();
            _mockLogger = new Mock<ILogger<InvoiceItemController>>();
            _controller = new InvoiceItemController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetByInvoiceId_ReturnsNotFound_WhenNoItems()
        {
            int invoiceId = 1;
            _mockService.Setup(s => s.GetByInvoiceIdAsync(invoiceId))
                        .ReturnsAsync(new List<InvoiceItemDto>());

            var result = await _controller.GetByInvoiceId(invoiceId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Nenhum item encontrado para a fatura com ID {invoiceId}.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetByInvoiceId_ReturnsOk_WhenItemsExist()
        {
            int invoiceId = 1;
            var items = new List<InvoiceItemDto>
            {
                new InvoiceItemDto { ProductCode = "PROD1", Quantity = 2 },
                new InvoiceItemDto { ProductCode = "PROD2", Quantity = 5 }
            };

            _mockService.Setup(s => s.GetByInvoiceIdAsync(invoiceId))
                        .ReturnsAsync(items);

            var result = await _controller.GetByInvoiceId(invoiceId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<IEnumerable<InvoiceItemDto>>(okResult.Value);
            Assert.Equal(2, returnedItems.Count());
        }

        [Fact]
        public async Task GetByInvoiceId_HandlesException_ReturnsInternalServerError()
        {
            int invoiceId = 1;
            _mockService.Setup(s => s.GetByInvoiceIdAsync(invoiceId))
                        .ThrowsAsync(new System.Exception("Banco de dados indisponível"));

            var result = await _controller.GetByInvoiceId(invoiceId);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("Erro interno ao buscar itens da fatura.", statusResult.Value);
        }
    }
}
