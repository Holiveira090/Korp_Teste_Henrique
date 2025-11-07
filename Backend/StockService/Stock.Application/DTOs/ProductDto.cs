using System.ComponentModel.DataAnnotations;

namespace Stock.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O código do produto é obrigatório.")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string Description { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
    public class StockUpdateDto
    {
        public int QuantityChange { get; set; }
    }
}
