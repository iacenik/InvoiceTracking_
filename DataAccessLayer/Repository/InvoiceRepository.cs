using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetInvoicesByClientIdAsync(int clientId)
        {
            return await _context.Invoices
                .Where(i => i.ClientId == clientId)
                .Include(i => i.InvoiceDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetUnpaidInvoicesAsync()
        {
            return await _context.Invoices
                .Where(i => !i.IsPaid)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ApproveInvoiceAsync(Invoice invoice, CashRegister cashRegister)
        {
            if (!invoice.IsPaid)
            {
                cashRegister.DeductExpense(invoice.TotalAmount, invoice.Currency);
                invoice.IsPaid = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
