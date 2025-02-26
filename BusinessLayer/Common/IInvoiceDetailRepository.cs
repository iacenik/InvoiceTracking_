using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IInvoiceDetailRepository : IGenericRepository<InvoiceDetail>
    {
        Task<List<InvoiceDetail>> GetDetailsByInvoiceIdAsync(int invoiceId);
        Task<InvoiceDetail?> GetDetailByIdAsync(int invoiceDetailId);
    }
}
