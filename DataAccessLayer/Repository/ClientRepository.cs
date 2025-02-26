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
     public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        ApplicationDbContext _context;
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Client?> FindByClientCodeAsync(int clientCode)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClientCode == clientCode);
        }

        public async Task<bool> ClientExistsAsync(int clientCode)
        {
            return await _context.Clients.AnyAsync(c => c.ClientCode == clientCode);
        }

        public async Task<List<Invoice>> GetClientInvoicesAsync(int clientId)
        {
            return await _context.Invoices
                .Where(i => i.ClientId == clientId)
                .Include(i => i.InvoiceDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Payment>> GetClientPaymentsAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .Include(p => p.PaymentDetails)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
