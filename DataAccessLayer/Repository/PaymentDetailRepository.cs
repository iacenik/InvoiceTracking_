using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class PaymentDetailRepository : GenericRepository<PaymentDetail>, IPaymentDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // 📌 1️⃣ Belirli bir ödemeye ait tüm detayları getirir
        public async Task<List<PaymentDetail>> GetDetailsByPaymentIdAsync(int paymentId)
        {
            return await _context.PaymentDetails
                .Where(d => d.PaymentId == paymentId)
                .AsNoTracking() // Veriyi izleme, sadece oku (performansı artırır)
                .ToListAsync();
        }

        // 📌 2️⃣ Belirli bir ödeme detayını getirir
        public async Task<PaymentDetail?> GetDetailByIdAsync(int paymentDetailId)
        {
            return await _context.PaymentDetails
                .FirstOrDefaultAsync(d => d.PaymentDetailId == paymentDetailId);
        }
    }
}
