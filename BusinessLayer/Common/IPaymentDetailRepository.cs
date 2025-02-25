using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Common
{
    public interface IPaymentDetailRepository : IGenericRepository<PaymentDetail>
    {
        Task<IEnumerable<PaymentDetail>> GetAllPaymentDetailsAsync(); // Tüm ödeme detaylarını getir
        Task<PaymentDetail?> GetPaymentDetailByIdAsync(int paymentDetailId); // Belirli bir ödeme detayını getir
        Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByPaymentAsync(int paymentId); // Belirli bir ödemeye ait detayları getir
        Task AddPaymentDetailAsync(PaymentDetail paymentDetail); // Yeni ödeme detayı ekle
        Task UpdatePaymentDetailAsync(PaymentDetail paymentDetail); // Ödeme detayını güncelle
        Task DeletePaymentDetailAsync(int paymentDetailId); // Ödeme detayını sil

        Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByCurrencyAsync(CurrencyType currency);
    }
}
