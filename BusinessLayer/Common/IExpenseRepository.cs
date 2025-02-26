using EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<List<Expense>> GetExpensesByEmployeeIdAsync(int employeeId);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(int categoryId);
    }
}
