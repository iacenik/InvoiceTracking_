using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IPaymentDetailRepository : IGenericRepository<PaymentDetail>
    {
        Task<List<PaymentDetail>> GetDetailsByPaymentIdAsync(int paymentId);
        Task<PaymentDetail?> GetDetailByIdAsync(int paymentDetailId);
    }
}
