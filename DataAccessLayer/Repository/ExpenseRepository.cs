using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace DataAccessLayer.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public ExpenseRepository(ApplicationDbContext context, ICashRegisterRepository cashRegisterRepository) : base(context)
        {
            _context = context;
            _cashRegisterRepository = cashRegisterRepository;
        }

        // 📌 Tüm giderleri getir
        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .ToListAsync();
        }

        // 📌 Belirli bir gideri getir
        public async Task<Expense?> GetExpenseByIdAsync(int expenseId)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);
        }

        // 📌 Çalışana göre giderleri getir
        public async Task<IEnumerable<Expense>> GetExpensesByEmployeeAsync(int employeeId)
        {
            return await _context.Expenses
                .Where(e => e.EmployeeId == employeeId)
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .ToListAsync();
        }

        // 📌 Kategoriye göre giderleri getir
        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _context.Expenses
                .Where(e => e.CategoryId == categoryId)
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .ToListAsync();
        }

        // 📌 Tarih aralığına göre giderleri getir
        public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .Include(e => e.Employee)
                .Include(e => e.Category)
                .ToListAsync();
        }

        // 📌 Yeni Gider Ekleme İşlemi
        public async Task AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            // ✅ Gider kasadan düşülecek
            await _cashRegisterRepository.AddExpenseAsync(expense.Amount, expense.Currency);
        }

        // 📌 Gider Güncelleme İşlemi
        public async Task UpdateExpenseAsync(Expense expense)
        {
            var existingExpense = await _context.Expenses.FindAsync(expense.ExpenseId);
            if (existingExpense != null)
            {
                // ✅ Eski gideri kasaya geri ekle
                await _cashRegisterRepository.AddIncomeAsync(existingExpense.Amount, existingExpense.Currency);

                // ✅ Güncellenen gideri kaydet
                _context.Entry(existingExpense).CurrentValues.SetValues(expense);
                await _context.SaveChangesAsync();

                // ✅ Yeni gideri kasadan düş
                await _cashRegisterRepository.AddExpenseAsync(expense.Amount, expense.Currency);
            }
        }

        // 📌 Gider Silme İşlemi
        public async Task DeleteExpenseAsync(int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            if (expense != null)
            {
                // ✅ Silinen gideri kasaya geri ekle
                await _cashRegisterRepository.AddIncomeAsync(expense.Amount, expense.Currency);

                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }
    }
}
