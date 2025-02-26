using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // 📌 1️⃣ Belirli bir faturaya ait tüm detayları getirir
        public async Task<List<InvoiceDetail>> GetDetailsByInvoiceIdAsync(int invoiceId)
        {
            return await _context.InvoiceDetails
                .Where(d => d.InvoiceId == invoiceId)
                .AsNoTracking() // Veriyi izleme, sadece oku (performansı artırır)
                .ToListAsync();
        }

        // 📌 2️⃣ Belirli bir fatura detayını getirir
        public async Task<InvoiceDetail?> GetDetailByIdAsync(int invoiceDetailId)
        {
            return await _context.InvoiceDetails
                .FirstOrDefaultAsync(d => d.InvoiceDetailId == invoiceDetailId);
        }
    }
}
