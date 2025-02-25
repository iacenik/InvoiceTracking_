using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Invoıces
                .Include(i => i.Client)
                .Include(i => i.Employee)
                .Include(i => i.Category)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoıces
                .Include(i => i.Client)
                .Include(i => i.Employee)
                .Include(i => i.Category)
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByClientAsync(int clientId)
        {
            return await _context.Invoıces
                .Where(i => i.ClientId == clientId)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByEmployeeAsync(int employeeId)
        {
            return await _context.Invoıces
                .Where(i => i.EmployeeId == employeeId)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Invoıces
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetPaidInvoicesAsync()
        {
            return await _context.Invoıces
                .Where(i => i.IsPaid)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync()
        {
            return await _context.Invoıces
                .Where(i => !i.IsPaid)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByTypeAsync(Enums.InvoiceType invoiceType)
        {
            return await _context.Invoıces
                .Where(i => i.InvoiceType == invoiceType)
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetUnpaidInvoicesByClientAsync(int clientId)
        {
            return await _context.Invoıces
                .Where(i => i.ClientId == clientId && !i.IsPaid)
                .Include(i => i.InvoiceDetails)
                .ToListAsync();
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            invoice.IsPaid = false; // Varsayılan olarak false ayarla
            await _context.Invoıces.AddAsync(invoice); // "Invoıces" -> "Invoices" olarak düzeltildi
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            _context.Invoıces.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(int invoiceId)
        {
            var invoice = await _context.Invoıces.FindAsync(invoiceId);
            if (invoice != null)
            {
                _context.Invoıces.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}
