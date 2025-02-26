using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetExpensesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Expenses
                .Where(e => e.EmployeeId == employeeId)
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            return await _context.Expenses
                .Where(e => e.CategoryId == categoryId)
                .Include(e => e.Employee)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
