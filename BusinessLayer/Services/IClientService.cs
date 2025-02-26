using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int clientId);
        Task<Client?> GetClientByCodeAsync(int clientCode);
        Task<List<Invoice>> GetClientInvoicesAsync(int clientId);
        Task<List<Payment>> GetClientPaymentsAsync(int clientId);
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int clientId);
    }
}
