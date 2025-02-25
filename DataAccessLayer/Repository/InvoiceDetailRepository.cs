using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
     public class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceDetailRepository( ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<InvoiceDetail>> GetAllInvoiceDetailsAsync()
        {
            return await _context.InvoiceDetails
                .Include(id => id.Invoice) // Fatura bilgilerini dahil et
                .Include(id => id.Item) // Ürün bilgilerini dahil et
                .ToListAsync();
        }

        public async Task<InvoiceDetail?> GetInvoiceDetailByIdAsync(int invoiceDetailId)
        {
            return await _context.InvoiceDetails
                .Include(id => id.Invoice)
                .Include(id => id.Item)
                .FirstOrDefaultAsync(id => id.InvoiceDetailId == invoiceDetailId);
        }

        public async Task<IEnumerable<InvoiceDetail>> GetInvoiceDetailsByInvoiceAsync(int invoiceId)
        {
            return await _context.InvoiceDetails
                .Where(id => id.InvoiceId == invoiceId)
                .Include(id => id.Item)
                .ToListAsync();
        }

        public async Task<IEnumerable<InvoiceDetail>> GetInvoiceDetailsByItemAsync(int itemId)
        {
            return await _context.InvoiceDetails
                .Where(id => id.ItemId == itemId)
                .Include(id => id.Invoice)
                .ToListAsync();
        }

        public async Task AddInvoiceDetailAsync(InvoiceDetail invoiceDetail)
        {
            invoiceDetail.CalculateTotalPrice(); // Toplam fiyatı hesapla
            await _context.InvoiceDetails.AddAsync(invoiceDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInvoiceDetailAsync(InvoiceDetail invoiceDetail)
        {
            invoiceDetail.CalculateTotalPrice(); // Güncellemeden önce toplam fiyatı hesapla
            _context.InvoiceDetails.Update(invoiceDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceDetailAsync(int invoiceDetailId)
        {
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(invoiceDetailId);
            if (invoiceDetail != null)
            {
                _context.InvoiceDetails.Remove(invoiceDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
