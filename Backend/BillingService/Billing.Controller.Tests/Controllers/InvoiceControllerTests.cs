using Billing.Application.DTOs;
using Billing.Application.Services.Interfaces;
using Billing.Controller.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Billing.Controller.Tests.Controllers
{
    public class InvoiceControllerTests
    {
        private readonly Mock<IInvoiceService> _mockService;
        private readonly Mock<ILogger<InvoiceController>> _mockLogger;
        private readonly InvoiceController _controller;

        public InvoiceControllerTests()
        {
            _mockService = new Mock<IInvoiceService>();
            _mockLogger = new Mock<ILogger<InvoiceController>>();

            _controller = new InvoiceController(_mockService.Object, _mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }


        [Fact]
        public async Task GetBySequentialNumber_InvoiceExists_ReturnsOk()
        {
            var dto = new InvoiceDTO { Id = 1, SequentialNumber = 1001 };
            _mockService.Setup(s => s.GetBySequentialNumberAsync(1001)).ReturnsAsync(dto);

            var result = await _controller.GetBySequentialNumber(1001);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<InvoiceDTO>(okResult.Value);
            Assert.Equal(1001, returnedDto.SequentialNumber);
        }

        [Fact]
        public async Task GetBySequentialNumber_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetBySequentialNumberAsync(9999)).ReturnsAsync((InvoiceDTO?)null);

            var result = await _controller.GetBySequentialNumber(9999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Fatura com número sequencial 9999 não encontrada.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetBySequentialNumber_Exception_Returns500()
        {
            _mockService.Setup(s => s.GetBySequentialNumberAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Erro"));

            var result = await _controller.GetBySequentialNumber(1);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno ao buscar a fatura.", objectResult.Value);
        }


        [Fact]
        public async Task GetInvoiceWithItems_InvoiceExists_ReturnsOk()
        {
            var dto = new InvoiceDTO { Id = 1, SequentialNumber = 123 };
            _mockService.Setup(s => s.GetInvoiceWithItemsAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetInvoiceWithItems(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<InvoiceDTO>(okResult.Value);
            Assert.Equal(1, returnedDto.Id);
        }

        [Fact]
        public async Task GetInvoiceWithItems_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetInvoiceWithItemsAsync(99)).ReturnsAsync((InvoiceDTO?)null);

            var result = await _controller.GetInvoiceWithItems(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Fatura com ID 99 não encontrada.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetInvoiceWithItems_Exception_Returns500()
        {
            _mockService.Setup(s => s.GetInvoiceWithItemsAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Erro interno"));

            var result = await _controller.GetInvoiceWithItems(1);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno ao buscar a fatura com itens.", objectResult.Value);
        }


        [Fact]
        public async Task PrintInvoice_Success_ReturnsOk()
        {
            var dto = new InvoiceDTO { Id = 1, Status = "Fechada" };
            _mockService.Setup(s => s.PrintInvoiceAsync(1)).ReturnsAsync(dto);

            var result = await _controller.PrintInvoice(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<InvoiceDTO>(okResult.Value);
            Assert.Equal("Fechada", returnedDto.Status);
        }

        [Fact]
        public async Task PrintInvoice_Exception_ReturnsBadRequest()
        {
            _mockService.Setup(s => s.PrintInvoiceAsync(1)).ThrowsAsync(new Exception("Erro ao imprimir"));

            var result = await _controller.PrintInvoice(1);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao imprimir", badRequest.Value);
        }
    }
}
