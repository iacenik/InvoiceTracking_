using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public  interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync(); // Tüm faturaları getir
        Task<Invoice?> GetInvoiceByIdAsync(int invoiceId); // Belirli bir faturayı getir
        Task<IEnumerable<Invoice>> GetInvoicesByClientAsync(int clientId); // Müşteriye ait faturaları getir
        Task<IEnumerable<Invoice>> GetInvoicesByEmployeeAsync(int employeeId); // Çalışanın oluşturduğu faturaları getir
        Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate); // Tarih aralığına göre faturaları getir
        Task<IEnumerable<Invoice>> GetPaidInvoicesAsync(); // Ödenmiş faturaları getir
        Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync(); // Ödenmemiş faturaları getir
        Task AddInvoiceAsync(Invoice invoice); // Yeni fatura ekle
        Task UpdateInvoiceAsync(Invoice invoice); // Faturayı güncelle
        Task DeleteInvoiceAsync(int invoiceId); // Faturayı sil
    }
}
