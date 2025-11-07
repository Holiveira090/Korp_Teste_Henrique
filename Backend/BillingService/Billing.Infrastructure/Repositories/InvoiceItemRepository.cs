using Billing.Domain.Interfaces;
using Billing.Domain.Models;
using Billing.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories
{
    public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(BillingDbContext context) : base(context) { }

        public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId)
        {
            return await _context.InvoiceItems
                .Where(ii => ii.InvoiceId == invoiceId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
