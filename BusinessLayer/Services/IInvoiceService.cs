using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IInvoiceService
    {
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<List<Invoice>> GetInvoicesByClientIdAsync(int clientId);
        Task<List<Invoice>> GetUnpaidInvoicesAsync();
        Task ApproveInvoiceAsync(int invoiceId, CashRegister cashRegister);
        Task AddInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int invoiceId);
    }
}
