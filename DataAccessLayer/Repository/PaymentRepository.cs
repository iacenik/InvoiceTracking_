using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace DataAccessLayer.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetPaymentsByClientIdAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .Include(p => p.PaymentDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ProcessPaymentAsync(Payment payment, CashRegister cashRegister)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Kasa güncelleme işlemi
            switch (payment.Currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalIncomeEUR += payment.Amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalIncomeRON += payment.Amount;
                    break;
                case CurrencyType.USD:
                    cashRegister.TotalIncomeUSD += payment.Amount;
                    break;
            }
            await _context.SaveChangesAsync();
        }
    }
}
