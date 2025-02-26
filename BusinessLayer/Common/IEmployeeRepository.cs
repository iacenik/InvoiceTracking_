using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<bool> EmployeeExistsAsync(int employeeId);
        Task<List<Expense>> GetEmployeeExpensesAsync(int employeeId);
    }
}
