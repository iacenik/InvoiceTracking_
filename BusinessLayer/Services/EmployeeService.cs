using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return (await _employeeRepository.GetAllAsync()).ToList();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        }

        public async Task<List<Expense>> GetEmployeeExpensesAsync(int employeeId)
        {
            return await _employeeRepository.GetEmployeeExpensesAsync(employeeId);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (await _employeeRepository.EmployeeExistsAsync(employee.EmployeeId))
            {
                throw new Exception("Bu çalışan zaten mevcut!");
            }
            await _employeeRepository.AddAsync(employee);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee != null)
            {
                await _employeeRepository.DeleteAsync(employee);
            }
        }

        public async Task AddExpenseForEmployeeAsync(int employeeId, Expense expense, CashRegister cashRegister)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Çalışan bulunamadı!");

            // Harcamayı çalışana ekle
            employee.Expenses.Add(expense);
            await _employeeRepository.UpdateAsync(employee);

            // Kasa güncelleme işlemi
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
        }
    }
}
