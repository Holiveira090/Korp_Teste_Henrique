using Billing.Application.DTOs;

namespace Billing.Application.Services.Interfaces
{
    public interface IInvoiceService : IServiceBase<InvoiceDTO>
    {
        Task<InvoiceDTO?> GetBySequentialNumberAsync(int sequentialNumber);
        Task<int> GetNextSequentialNumberAsync();
        Task<InvoiceDTO?> GetInvoiceWithItemsAsync(int id);
        Task<InvoiceDTO> PrintInvoiceAsync(int invoiceId);
    }
}
