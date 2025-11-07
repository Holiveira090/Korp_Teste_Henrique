using Billing.Domain.Models;

namespace Billing.Domain.Interfaces
{
    public interface IInvoiceItemRepository : IRepositoryBase<InvoiceItem>
    {
        Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId);
    }
}
