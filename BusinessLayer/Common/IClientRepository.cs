using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
     public interface IClientRepository : IGenericRepository<Client>
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(); // Tüm müşterileri getir
        Task<Client?> GetClientByIdAsync(int clientId); // Belirli bir müşteriyi getir
        Task<Client?> GetClientByCodeAsync(int clientCode); // ClientCode'a göre getir
        Task<IEnumerable<Invoice>> GetClientInvoicesAsync(int clientId); // Müşterinin faturalarını getir
        Task<IEnumerable<Payment>> GetClientPaymentsAsync(int clientId); // Müşterinin yaptığı ödemeleri getir
        Task AddClientAsync(Client client); // Yeni müşteri ekle
        Task UpdateClientAsync(Client client); // Müşteri bilgilerini güncelle
        Task DeleteClientAsync(int clientId); // Müşteriyi sil
    }
}
