using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
    {
        Task<IEnumerable<ExpenseCategory>> GetAllExpenseCategoriesAsync(); // Tüm gider kategorilerini getir
        Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(int categoryId); // Belirli bir gider kategorisini getir
        Task AddExpenseCategoryAsync(ExpenseCategory category); // Yeni gider kategorisi ekle
        Task UpdateExpenseCategoryAsync(ExpenseCategory category); // Gider kategorisini güncelle
        Task DeleteExpenseCategoryAsync(int categoryId); // Gider kategorisini sil

    }
}
