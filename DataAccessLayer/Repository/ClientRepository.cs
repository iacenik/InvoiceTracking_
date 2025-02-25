using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
     public class ClientRepository : GenericRepository<Client> , IClientRepository
    {
        private readonly ApplicationDbContext _context;
        public ClientRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clıents
                .Include(c => c.Invoices) // Müşterinin faturalarını dahil et
                .Include(c => c.Payments) // Müşterinin ödemelerini dahil et
                .ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            return await _context.Clıents
                .Include(c => c.Invoices)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Client?> GetClientByCodeAsync(int clientCode)
        {
            return await _context.Clıents
                .Include(c => c.Invoices)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.ClientCode == clientCode);
        }

        public async Task<IEnumerable<Invoice>> GetClientInvoicesAsync(int clientId)
        {
            return await _context.Invoıces
                .Where(i => i.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetClientPaymentsAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .ToListAsync();
        }

        public async Task AddClientAsync(Client client)
        {
            await _context.Clıents.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            _context.Clıents.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _context.Clıents.FindAsync(clientId);
            if (client != null)
            {
                _context.Clıents.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

    }
}
