using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IExpenseCategoryService
    {
        Task<List<ExpenseCategory>> GetAllCategoriesAsync();
        Task<ExpenseCategory?> GetCategoryByIdAsync(int categoryId);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
        Task AddCategoryAsync(ExpenseCategory category);
        Task UpdateCategoryAsync(ExpenseCategory category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
