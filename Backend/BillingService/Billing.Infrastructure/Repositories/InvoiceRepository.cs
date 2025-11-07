using Billing.Domain.Interfaces;
using Billing.Domain.Models;
using Billing.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(BillingDbContext context) : base(context) { }

        public async Task<Invoice?> GetBySequentialNumberAsync(int sequentialNumber)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.SequentialNumber == sequentialNumber);
        }

        public async Task<int> GetNextSequentialNumberAsync()
        {
            var lastNumber = await _context.Invoices
                .OrderByDescending(i => i.SequentialNumber)
                .Select(i => i.SequentialNumber)
                .FirstOrDefaultAsync();

            return lastNumber + 1;
        }

        public async Task<Invoice?> GetInvoiceWithItemsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
