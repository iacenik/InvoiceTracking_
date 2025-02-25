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
     public class EmployeeRepository :GenericRepository<Employee>,IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Expenses) // Çalışanın giderlerini dahil et
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Expenses)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<IEnumerable<Expense>> GetEmployeeExpensesAsync(int employeeId)
        {
            return await _context.Expenses
                .Where(exp => exp.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

    }
}
