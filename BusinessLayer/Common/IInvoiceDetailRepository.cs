using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
     public interface IInvoiceDetailRepository :IGenericRepository<InvoiceDetail>
    {
        Task<IEnumerable<InvoiceDetail>> GetAllInvoiceDetailsAsync(); // Tüm fatura detaylarını getir
        Task<InvoiceDetail?> GetInvoiceDetailByIdAsync(int invoiceDetailId); // Belirli bir fatura detayını getir
        Task<IEnumerable<InvoiceDetail>> GetInvoiceDetailsByInvoiceAsync(int invoiceId); // Belirli bir faturaya ait detayları getir
        Task<IEnumerable<InvoiceDetail>> GetInvoiceDetailsByItemAsync(int itemId); // Belirli bir ürüne ait detayları getir
        Task AddInvoiceDetailAsync(InvoiceDetail invoiceDetail); // Yeni fatura detayı ekle
        Task UpdateInvoiceDetailAsync(InvoiceDetail invoiceDetail); // Fatura detayını güncelle
        Task DeleteInvoiceDetailAsync(int invoiceDetailId); // Fatura detayını sil
    }
}
