using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Services.Interfaces;

namespace Stock.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController<ProductDto>
    {
        private readonly IProductServices _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductServices productService, ILogger<ProductController> logger)
            : base(productService, logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return BadRequest("Código inválido.");

                var result = await _productService.GetByCodeAsync(code);
                if (result is null)
                    return NotFound($"Produto com código '{code}' não encontrado.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produto pelo código {Code}", code);
                return StatusCode(500, "Erro interno ao buscar produto.");
            }
        }

        [HttpPut("code/{productCode}/stock")]
        public async Task<IActionResult> UpdateStock(string productCode, [FromBody] StockUpdateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productCode))
                    return BadRequest("Código do produto inválido.");

                if (dto == null)
                    return BadRequest("Dados inválidos.");

                if (dto.QuantityChange == 0)
                    return BadRequest("QuantityChange deve ser diferente de zero.");

                var exists = await _productService.GetByCodeAsync(productCode);
                if (exists == null)
                    return NotFound($"Produto com código '{productCode}' não encontrado.");

                try
                {
                    var ok = await _productService.UpdateStockAsync(productCode, dto.QuantityChange);
                    if (!ok)
                        return BadRequest("Não foi possível atualizar o saldo.");
                }
                catch (InvalidOperationException invEx)
                {
                    _logger.LogWarning(invEx, "Falha ao atualizar saldo do produto {ProductCode}", productCode);
                    return BadRequest(invEx.Message);
                }

                var updated = await _productService.GetByCodeAsync(productCode);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar saldo do produto {ProductCode}", productCode);
                return StatusCode(500, "Erro interno ao atualizar saldo.");
            }
        }
    }
}