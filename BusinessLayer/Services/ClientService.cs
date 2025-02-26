using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
   public  class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<List<Client>> GetAllClientsAsync()
        {
            return (await _clientRepository.GetAllAsync()).ToList();
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            return await _clientRepository.GetByIdAsync(clientId);
        }

        public async Task<Client?> GetClientByCodeAsync(int clientCode)
        {
            return await _clientRepository.FindByClientCodeAsync(clientCode);
        }

        public async Task<List<Invoice>> GetClientInvoicesAsync(int clientId)
        {
            return await _clientRepository.GetClientInvoicesAsync(clientId);
        }

        public async Task<List<Payment>> GetClientPaymentsAsync(int clientId)
        {
            return await _clientRepository.GetClientPaymentsAsync(clientId);
        }

        public async Task AddClientAsync(Client client)
        {
            if (await _clientRepository.ClientExistsAsync(client.ClientCode))
            {
                throw new Exception("Bu müşteri kodu zaten var!");
            }
            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client != null)
            {
                await _clientRepository.DeleteAsync(client);
            }
        }
    }
}
