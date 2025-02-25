using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace DataAccessLayer.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public PaymentRepository(ApplicationDbContext context, ICashRegisterRepository cashRegisterRepository) : base(context)
        {
            _context = context;
            _cashRegisterRepository = cashRegisterRepository;
        }

        // 📌 Tüm ödemeleri getir
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Client)
                .Include(p => p.PaymentDetails)
                .ToListAsync();
        }

        // 📌 Belirli bir ödemeyi getir
        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Client)
                .Include(p => p.PaymentDetails)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        // 📌 Müşteriye göre ödemeleri getir
        public async Task<IEnumerable<Payment>> GetPaymentsByClientAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .Include(p => p.PaymentDetails)
                .ToListAsync();
        }

        // 📌 Tarih aralığına göre ödemeleri getir
        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.Date >= startDate && p.Date <= endDate)
                .Include(p => p.PaymentDetails)
                .ToListAsync();
        }

        // 📌 Yeni ödeme ekleme
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // ✅ Kasa güncelleme
            await _cashRegisterRepository.AddIncomeAsync(payment.Amount, payment.Currency);
        }

        // 📌 Ödeme Güncelleme İşlemi
        public async Task UpdatePaymentAsync(Payment payment)
        {
            var existingPayment = await _context.Payments.FindAsync(payment.PaymentId);
            if (existingPayment != null)
            {
                // ✅ Eski ödeme tutarını kasadan çıkar
                await _cashRegisterRepository.AddExpenseAsync(existingPayment.Amount, existingPayment.Currency);

                // ✅ Yeni ödeme bilgilerini kaydet
                _context.Entry(existingPayment).CurrentValues.SetValues(payment);
                await _context.SaveChangesAsync();

                // ✅ Güncellenmiş tutarı tekrar ekle
                await _cashRegisterRepository.AddIncomeAsync(payment.Amount, payment.Currency);
            }
        }

        // 📌 Ödeme Silme İşlemi
        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment != null)
            {
                // ✅ Ödeme silindiğinde CashRegister'dan düş
                await _cashRegisterRepository.AddExpenseAsync(payment.Amount, payment.Currency);

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        // 📌 Ödeme sonrası kasayı güncelle
        public async Task UpdateCashRegisterAfterPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.PaymentDetails)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment != null)
            {
                payment.CalculateAmount(); // 📌 Güncellenen ödeme detayları için toplam tutarı tekrar hesapla
                await _cashRegisterRepository.AddIncomeAsync(payment.Amount, payment.Currency);
                await _context.SaveChangesAsync();
            }
        }
    }
}
