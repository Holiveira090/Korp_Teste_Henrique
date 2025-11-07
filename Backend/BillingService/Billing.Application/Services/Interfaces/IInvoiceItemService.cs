using Billing.Application.DTOs;

namespace Billing.Application.Services.Interfaces
{
    public interface IInvoiceItemService : IServiceBase<InvoiceItemDto>
    {
        Task<IEnumerable<InvoiceItemDto>> GetByInvoiceIdAsync(int invoiceId);
    }
}
