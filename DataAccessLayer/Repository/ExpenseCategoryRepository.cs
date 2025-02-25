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
    public class ExpenseCategoryRepository : GenericRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<ExpenseCategory>> GetAllExpenseCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .Include(c => c.Expenses) // Bu kategorideki giderleri dahil et
                .ToListAsync();
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(int categoryId)
        {
            return await _context.ExpenseCategories
                .Include(c => c.Expenses)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task AddExpenseCategoryAsync(ExpenseCategory category)
        {
            await _context.ExpenseCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseCategoryAsync(ExpenseCategory category)
        {
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseCategoryAsync(int categoryId)
        {
            var category = await _context.ExpenseCategories.FindAsync(categoryId);
            if (category != null)
            {
                _context.ExpenseCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

    }
}
