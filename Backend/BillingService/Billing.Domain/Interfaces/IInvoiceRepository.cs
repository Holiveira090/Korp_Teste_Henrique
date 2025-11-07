using Billing.Domain.Models;

namespace Billing.Domain.Interfaces
{
    public interface IInvoiceRepository : IRepositoryBase<Invoice>
    {
        Task<Invoice?> GetBySequentialNumberAsync(int sequentialNumber);
        Task<int> GetNextSequentialNumberAsync();
        Task<Invoice?> GetInvoiceWithItemsAsync(int id);
    }
}
