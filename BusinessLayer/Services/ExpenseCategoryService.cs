using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _categoryRepository;

        public ExpenseCategoryService(IExpenseCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ExpenseCategory>> GetAllCategoriesAsync()
        {
            return (await _categoryRepository.GetAllAsync()).ToList();
        }

        public async Task<ExpenseCategory?> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetByIdAsync(categoryId);
        }

        public async Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            return await _categoryRepository.GetExpensesByCategoryIdAsync(categoryId);
        }

        public async Task AddCategoryAsync(ExpenseCategory category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(ExpenseCategory category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var expenses = await _categoryRepository.GetExpensesByCategoryIdAsync(categoryId);
            if (expenses.Any())
            {
                throw new Exception("Bu kategoriye bağlı giderler var. Önce giderleri silmelisiniz!");
            }

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }
        }
    }
}
