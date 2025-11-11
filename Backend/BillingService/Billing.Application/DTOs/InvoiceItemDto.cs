namespace Billing.Application.DTOs
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
    }
}
