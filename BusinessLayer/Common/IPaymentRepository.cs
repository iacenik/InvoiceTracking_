using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(); // 📌 Tüm ödemeleri getir
        Task<Payment?> GetPaymentByIdAsync(int paymentId); // 📌 Belirli bir ödemeyi getir
        Task<IEnumerable<Payment>> GetPaymentsByClientAsync(int clientId); // 📌 Belirli bir müşteriye ait ödemeleri getir
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate); // 📌 Tarih aralığına göre ödemeleri getir

        Task AddPaymentAsync(Payment payment); // 📌 Yeni ödeme ekle (Kasa ile entegre)
        Task UpdatePaymentAsync(Payment payment); // 📌 Ödemeyi güncelle (Eskiyi çıkar, yenisini ekle)
        Task DeletePaymentAsync(int paymentId); // 📌 Ödemeyi sil (Kasa ile entegre)

        Task UpdateCashRegisterAfterPaymentAsync(int paymentId); // 📌 Ödeme sonrası kasayı güncelle
    }
}
