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
    public class PaymentDetailRepository : GenericRepository<PaymentDetail>, IPaymentDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // 📌 Tüm Ödeme Detaylarını Getir
        public async Task<IEnumerable<PaymentDetail>> GetAllPaymentDetailsAsync()
        {
            return await _context.PaymentDetails
                .Include(pd => pd.Payment) // ✅ Ödeme bilgilerini getir
                .ToListAsync();
        }

        // 📌 Tek Bir Ödeme Detayı Getir
        public async Task<PaymentDetail?> GetPaymentDetailByIdAsync(int paymentDetailId)
        {
            return await _context.PaymentDetails
                .Include(pd => pd.Payment)
                .FirstOrDefaultAsync(pd => pd.PaymentDetailId == paymentDetailId);
        }

        // 📌 Belirli Bir `PaymentId` İçin Ödeme Detaylarını Getir
        public async Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByPaymentAsync(int paymentId)
        {
            return await _context.PaymentDetails
                .Where(pd => pd.PaymentId == paymentId)
                .Include(pd => pd.Payment) // ✅ Bağlı ödeme bilgilerini getir
                .ToListAsync();
        }

        // 📌 Yeni Ödeme Detayı Ekle
        public async Task AddPaymentDetailAsync(PaymentDetail paymentDetail)
        {
            paymentDetail.CalculateTotalAmount(); // ✅ Toplam tutarı hesapla
            await _context.PaymentDetails.AddAsync(paymentDetail);
            await _context.SaveChangesAsync();
        }

        // 📌 Ödeme Detayı Güncelle
        public async Task UpdatePaymentDetailAsync(PaymentDetail paymentDetail)
        {
            paymentDetail.CalculateTotalAmount(); // ✅ Güncellemeden önce hesapla
            _context.PaymentDetails.Update(paymentDetail);
            await _context.SaveChangesAsync();
        }

        // 📌 Ödeme Detayı Sil
        public async Task DeletePaymentDetailAsync(int paymentDetailId)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(paymentDetailId);
            if (paymentDetail != null)
            {
                _context.PaymentDetails.Remove(paymentDetail);
                await _context.SaveChangesAsync();
            }
        }

        // 📌 Belirli Bir Para Birimi ile Ödeme Detaylarını Getir
        public async Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByCurrencyAsync(CurrencyType currency)
        {
            return await _context.PaymentDetails
                .Where(pd => pd.Currency == currency)
                .Include(pd => pd.Payment) // ✅ Bağlı ödeme bilgilerini getir
                .ToListAsync();
        }
    }
}
