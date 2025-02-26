using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetAllExpensesAsync();
        Task<List<Expense>> GetExpensesByEmployeeIdAsync(int employeeId);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
        Task AddExpenseAsync(Expense expense, CashRegister cashRegister);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int expenseId);
        Task<Expense?> GetExpenseByIdAsync(int id); // 💡 Eksik olan metodu ekledik
    }
}
