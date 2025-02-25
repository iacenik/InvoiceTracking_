using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IItemRepository : IGenericRepository<Item> 
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(); // Tüm ürünleri getir
        Task<Item?> GetItemByIdAsync(int itemId); // Belirli bir ürünü getir
        Task<IEnumerable<Item>> GetItemsByInvoiceAsync(int invoiceId); // Belirli bir faturaya ait ürünleri getir
        Task AddItemAsync(Item item); // Yeni ürün ekle
        Task UpdateItemAsync(Item item); // Ürünü güncelle
        Task DeleteItemAsync(int itemId); // Ürünü sil
    }
}
