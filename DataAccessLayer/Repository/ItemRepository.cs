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
    public  class ItemRepository : GenericRepository<Item>,IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items
                .Include(i => i.InvoiceDetails) // Ürüne bağlı fatura detaylarını getir
                .ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            return await _context.Items
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<IEnumerable<Item>> GetItemsByInvoiceAsync(int invoiceId)
        {
            return await _context.InvoiceDetails
                .Where(id => id.InvoiceId == invoiceId)
                .Include(id => id.Item)
                .Select(id => id.Item!)
                .ToListAsync();
        }

        public async Task AddItemAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
