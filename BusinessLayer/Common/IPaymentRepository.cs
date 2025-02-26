using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetPaymentsByClientIdAsync(int clientId);
        Task ProcessPaymentAsync(Payment payment, CashRegister cashRegister);
    }
}
