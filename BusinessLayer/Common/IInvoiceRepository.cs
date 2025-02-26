using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<List<Invoice>> GetInvoicesByClientIdAsync(int clientId);
        Task<List<Invoice>> GetUnpaidInvoicesAsync();
        Task ApproveInvoiceAsync(Invoice invoice, CashRegister cashRegister);
    }
}
