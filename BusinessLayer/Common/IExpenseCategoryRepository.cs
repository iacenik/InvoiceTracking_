using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
    {
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
    }
}
