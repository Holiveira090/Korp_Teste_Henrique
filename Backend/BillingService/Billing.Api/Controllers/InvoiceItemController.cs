using Billing.Application.DTOs;
using Billing.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceItemController : BaseController<InvoiceItemDto>
    {
        private readonly IInvoiceItemService _invoiceItemService;

        public InvoiceItemController(IInvoiceItemService invoiceItemService, ILogger<InvoiceItemController> logger)
            : base(invoiceItemService, logger)
        {
            _invoiceItemService = invoiceItemService;
        }

        [HttpGet("ByInvoice/{invoiceId:int}")]
        public async Task<IActionResult> GetByInvoiceId(int invoiceId)
        {
            try
            {
                var items = await _invoiceItemService.GetByInvoiceIdAsync(invoiceId);
                if (items == null || !items.Any())
                    return NotFound($"Nenhum item encontrado para a fatura com ID {invoiceId}.");

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar itens da fatura com ID {InvoiceId}", invoiceId);
                return StatusCode(500, "Erro interno ao buscar itens da fatura.");
            }
        }
    }
}
