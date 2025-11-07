using Microsoft.AspNetCore.Mvc;
using Stock.Application.Services.Interfaces;

namespace Stock.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TDto> : ControllerBase where TDto : class
    {
        private readonly IServicesBase<TDto> _service;
        private readonly ILogger<BaseController<TDto>> _logger;

        public BaseController(IServicesBase<TDto> service, ILogger<BaseController<TDto>> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os registros de {Entity}", typeof(TDto).Name);
                return StatusCode(500, "Ocorreu um erro ao buscar os registros.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result is null)
                    return NotFound($"Registro com ID {id} não encontrado.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar registro de {Entity} com ID {Id}", typeof(TDto).Name, id);
                return StatusCode(500, "Erro interno ao buscar o registro.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Dados inválidos.");

                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar novo registro de {Entity}", typeof(TDto).Name);
                return StatusCode(500, "Erro interno ao criar o registro.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Dados inválidos.");

                var dtoType = dto.GetType();
                var idProp = dtoType.GetProperty("Id");
                if (idProp != null)
                    idProp.SetValue(dto, id);

                var updated = await _service.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar registro de {Entity} com ID {Id}", typeof(TDto).Name, id);
                return StatusCode(500, "Erro interno ao atualizar o registro.");
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound($"Registro com ID {id} não encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir registro de {Entity} com ID {Id}", typeof(TDto).Name, id);
                return StatusCode(500, "Erro interno ao excluir o registro.");
            }
        }
    }
}