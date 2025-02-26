using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IInvoiceDetailService
    {
        Task<List<InvoiceDetail>> GetAllInvoiceDetailsAsync();
        Task<List<InvoiceDetail>> GetDetailsByInvoiceIdAsync(int invoiceId);
        Task<InvoiceDetail?> GetDetailByIdAsync(int invoiceDetailId);
        Task AddInvoiceDetailAsync(InvoiceDetail invoiceDetail);
        Task UpdateInvoiceDetailAsync(InvoiceDetail invoiceDetail);
        Task DeleteInvoiceDetailAsync(int invoiceDetailId);
    }
}
