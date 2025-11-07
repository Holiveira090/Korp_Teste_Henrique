using Billing.Domain.Models.Enums;

namespace Billing.Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int SequentialNumber { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Aberta;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<InvoiceItem> Items { get; set; } = new();
    }
}
