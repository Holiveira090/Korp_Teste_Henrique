using Billing.Application.DTOs;
using Billing.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : BaseController<InvoiceDTO>
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
            : base(invoiceService, logger)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("BySequentialNumber/{sequentialNumber:int}")]
        public async Task<IActionResult> GetBySequentialNumber(int sequentialNumber)
        {
            try
            {
                var invoice = await _invoiceService.GetBySequentialNumberAsync(sequentialNumber);
                if (invoice == null)
                    return NotFound($"Fatura com número sequencial {sequentialNumber} não encontrada.");

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar fatura com número sequencial {SequentialNumber}", sequentialNumber);
                return StatusCode(500, "Erro interno ao buscar a fatura.");
            }
        }

        [HttpGet("WithItems/{id:int}")]
        public async Task<IActionResult> GetInvoiceWithItems(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceWithItemsAsync(id);
                if (invoice == null)
                    return NotFound($"Fatura com ID {id} não encontrada.");

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar fatura com itens para ID {Id}", id);
                return StatusCode(500, "Erro interno ao buscar a fatura com itens.");
            }
        }

        [HttpPut("{id:int}/print")]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            try
            {
                var printedInvoice = await _invoiceService.PrintInvoiceAsync(id);
                return Ok(printedInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao imprimir nota fiscal {InvoiceId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
