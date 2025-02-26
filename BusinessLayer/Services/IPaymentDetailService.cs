using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IPaymentDetailService
    {
        Task<List<PaymentDetail>> GetAllPaymentDetailsAsync();
        Task<List<PaymentDetail>> GetDetailsByPaymentIdAsync(int paymentId);
        Task<PaymentDetail?> GetDetailByIdAsync(int paymentDetailId);
        Task AddPaymentDetailAsync(PaymentDetail paymentDetail);
        Task UpdatePaymentDetailAsync(PaymentDetail paymentDetail);
        Task DeletePaymentDetailAsync(int paymentDetailId);
    }
}
