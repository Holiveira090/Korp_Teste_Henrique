namespace Billing.Application.DTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int SequentialNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
