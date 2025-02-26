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
        Task<Client?> FindByClientCodeAsync(int clientCode);
        Task<bool> ClientExistsAsync(int clientCode);
        Task<List<Invoice>> GetClientInvoicesAsync(int clientId);
        Task<List<Payment>> GetClientPaymentsAsync(int clientId);
    }
}
