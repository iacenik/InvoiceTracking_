using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
   public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync(); // Tüm giderleri getir
        Task<Expense?> GetExpenseByIdAsync(int expenseId); // Belirli bir gideri getir
        Task<IEnumerable<Expense>> GetExpensesByEmployeeAsync(int employeeId); // Çalışana ait giderleri getir
        Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId); // Belirli bir kategoriye ait giderleri getir
        Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate); // Tarih aralığına göre giderleri getir
        Task AddExpenseAsync(Expense expense); // Yeni gider ekle
        Task UpdateExpenseAsync(Expense expense); // Gideri güncelle
        Task DeleteExpenseAsync(int expenseId); // Gideri sil
    }
}
