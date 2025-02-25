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
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(); // Tüm çalışanları getir
        Task<Employee?> GetEmployeeByIdAsync(int employeeId); // Belirli bir çalışanı getir
        Task<IEnumerable<Expense>> GetEmployeeExpensesAsync(int employeeId); // Çalışanın yaptığı giderleri getir
        Task AddEmployeeAsync(Employee employee); // Yeni çalışan ekle
        Task UpdateEmployeeAsync(Employee employee); // Çalışan bilgilerini güncelle
        Task DeleteEmployeeAsync(int employeeId); // Çalışanı sil
    }
}
