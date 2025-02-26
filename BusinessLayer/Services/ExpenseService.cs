using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public ExpenseService(IExpenseRepository expenseRepository, ICashRegisterRepository cashRegisterRepository)
        {
            _expenseRepository = expenseRepository;
            _cashRegisterRepository = cashRegisterRepository;
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            return (await _expenseRepository.GetAllAsync()).ToList();
        }
        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _expenseRepository.GetByIdAsync(id); // 💡 Repository içindeki metodu çağırıyoruz
        }


        public async Task<List<Expense>> GetExpensesByEmployeeIdAsync(int employeeId)
        {
            return await _expenseRepository.GetExpensesByEmployeeIdAsync(employeeId);
        }

        public async Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            return await _expenseRepository.GetExpensesByCategoryIdAsync(categoryId);
        }

        public async Task AddExpenseAsync(Expense expense, CashRegister cashRegister)
        {
            await _expenseRepository.AddAsync(expense);

            // 📌 Kasa güncelleme işlemi
            switch (expense.Currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalExpenseEUR += expense.Amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalExpenseRON += expense.Amount;
                    break;
                case CurrencyType.USD:
                    cashRegister.TotalExpenseUSD += expense.Amount;
                    break;
            }

            await _cashRegisterRepository.UpdateAsync(cashRegister);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            await _expenseRepository.UpdateAsync(expense);
        }

        public async Task DeleteExpenseAsync(int expenseId)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId);
            if (expense != null)
            {
                await _expenseRepository.DeleteAsync(expense);
            }
        }
    }
}
